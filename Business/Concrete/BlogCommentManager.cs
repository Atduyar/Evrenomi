using System;
using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using Business.Constants;
using Business.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Extensions;

namespace Business.Concrete
{
    public class BlogCommentManager:IBlogCommentService
    {
        private IBlogCommentDal _blogCommentDal;
        private IUserService _userService;
        private IBlogService _blogService;
        private IDateTimeHelper _dateTimeHelper;

        public BlogCommentManager(IBlogCommentDal blogCommentDal, IUserService userService, IBlogService blogService, IDateTimeHelper dateTimeHelper)
        {
            _blogCommentDal = blogCommentDal;
            _userService = userService;
            _blogService = blogService;
            _dateTimeHelper = dateTimeHelper;
        }

        public IDataResult<BlogComment> GetById(int blogCommentId)
        {
            return new SuccessDataResult<BlogComment>(_blogCommentDal.Get(p => p.Id == blogCommentId && p.ParentBlogCommentId == null));
        }

        public IDataResult<List<BlogComment>> GetByBlogId(int blogId)
        {
            return new SuccessDataResult<List<BlogComment>>(_blogCommentDal.GetList(p => p.BlogId == blogId && p.ParentBlogCommentId == null));
        }

        public IDataResult<List<BlogComment>> GetByUserId(int userId)
        {
            return new SuccessDataResult<List<BlogComment>>(_blogCommentDal.GetList(p => p.UserId == userId && p.ParentBlogCommentId == null));
        }

        public IDataResult<List<BlogComment>> GetByStatus(int status, int mask)
        {
            return new SuccessDataResult<List<BlogComment>>(_blogCommentDal.GetList(p => ((status ^ p.Status) & mask) != 0 ));//int result = (filter ^ x) & mask;
            //return new SuccessDataResult<List<BlogComment>>(_blogCommentDal.GetList(p => p.Status == status));
        }

        public IDataResult<List<BlogComment>> GetByCommentResponse(int blogCommentId)
        {
            return new SuccessDataResult<List<BlogComment>>(_blogCommentDal.GetList(p => p.ParentBlogCommentId == blogCommentId));
        }

        public IDataResult<CommentForBlog> GetByCommentIdForBlog(int blogCommentId)
        {
            var c = GetById(blogCommentId).Data;
            var user = _userService.GetById(c.UserId,Status.Per.System);
            if (!user.Success)
            {
                return new ErrorDataResult<CommentForBlog>();
            }

            var result = new CommentForBlog
                {
                    CommentId = c.Id,
                    CommentDate = _dateTimeHelper.SetTime(c.CommentDate),
                    CommentResponse = GetByCommentResponse(c.Id).Data.Count,
                    Text = c.Text,
                    UserId = c.UserId,
                    UserNickname = user.Data.ToDetail().Nickname,
                    UserAvatarUrl = user.Data.ToDetail().AvatarUrl
                };

            return new SuccessDataResult<CommentForBlog>(result);
        }

        public IDataResult<List<CommentForBlog>> GetByCommentForBlog(int blogId)
        {
            var blog = _blogService.GetById(blogId, Status.Per.User);
            if (!blog.Success || blog.Data == null)
            {
                return new ErrorDataResult<List<CommentForBlog>>(message:Messages.BlogNotFound);
            }
            var comments= GetByBlogId(blogId);

            var result = from c in comments.Data
                select new CommentForBlog
                {
                    CommentId = c.Id,
                    CommentDate = _dateTimeHelper.SetTime(c.CommentDate),
                    CommentResponse = GetByCommentResponse(c.Id).Data.Count,
                    Text = c.Text,
                    UserId = c.UserId,
                    UserNickname = _userService.GetById(c.UserId,Status.Per.System).Data.ToDetail().Nickname,
                    UserAvatarUrl = _userService.GetById(c.UserId,Status.Per.System).Data.ToDetail().AvatarUrl
                };

            return new SuccessDataResult<List<CommentForBlog>>(result.ToList());
        }

        public IDataResult<List<CommentForBlog>> GetByCommentResponseForBlog(int blogCommentId)
        {
            var commentResponses = GetByCommentResponse(blogCommentId);

            var result = from c in commentResponses.Data
                select new CommentForBlog
                {
                    CommentId = c.Id,
                    CommentDate = _dateTimeHelper.SetTime(c.CommentDate),
                    CommentResponse = GetByCommentResponse(c.Id).Data.Count,
                    Text = c.Text,
                    UserId = c.UserId,
                    UserNickname = _userService.GetById(c.UserId,Status.Per.System).Data.ToDetail().Nickname,
                    UserAvatarUrl = _userService.GetById(c.UserId, Status.Per.System).Data.ToDetail().AvatarUrl
                };
            
            return new SuccessDataResult<List<CommentForBlog>>(result.ToList());
        }

        public IDataResult<List<BlogComment>> GetList()
        {
            return new SuccessDataResult<List<BlogComment>>(_blogCommentDal.GetList());
        }

        public IDataResult<int> Add(BlogComment blogComment, int userId)
        {
            var user = _userService.GetById(blogComment.UserId,Status.Per.System);
            if (user.Data == null)
            {
                return new ErrorDataResult<int>(message: Messages.UserNotFound);
            }

            var blog = _blogService.GetById(blogComment.BlogId,Status.Per.User);
            if (blog.Data == null)
            {
                return new ErrorDataResult<int>(message: Messages.BlogNotFound);
            }
            if (((Status.Neno ^ user.Data.Status) & Status.Sban[2]) != 0)//Profil banı var mı?
            {
                return new ErrorDataResult<int>(message: Messages.UserMessageBan);
            }

            return new SuccessDataResult<int>(_blogCommentDal.Add(blogComment), Messages.BlogCommentAdded);
        }

        public IResult Update(BlogComment blogComment)
        {
            var user = _userService.GetById(blogComment.UserId,Status.Per.System);
            if (user.Data == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            var blog = _blogService.GetById(blogComment.BlogId, Status.Per.User);
            if (blog.Data == null)
            {
                return new ErrorResult(Messages.BlogNotFound);
            }
            if (((Status.Neno ^ user.Data.Status) & Status.Sban[2]) != 0)//Profil banı var mı?
            {
                return new ErrorResult(Messages.UserMessageBan);
            }

            _blogCommentDal.Update(blogComment);
            return new SuccessResult(Messages.BlogCommentUpdated);
        }

        public IResult Delete(BlogComment blogComment)
        {
            var user = _userService.GetById(blogComment.UserId,Status.Per.System);
            if (user.Data == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            var blog = _blogService.GetById(blogComment.BlogId, Status.Per.User);
            if (blog.Data == null)
            {
                return new ErrorResult(Messages.BlogNotFound);
            }

            _blogCommentDal.Delete(blogComment);
            return new SuccessResult(Messages.BlogCommentDeleted);
        }

        public IDataResult<int> AddCommentForBlog(AddCommentForBlog addCommentForBlog, int userId)
        {
            var result = Add(new BlogComment
            {
                BlogId = addCommentForBlog.BlogId,
                ParentBlogCommentId = addCommentForBlog.ParentBlogCommentId,
                Text = addCommentForBlog.Text,
                CommentDate = DateTime.Now,
                Status = 0,
                UserId = userId
            }, userId);
            if (!result.Success)
            {
                return result;
            }
            return result;
        }

    }
}