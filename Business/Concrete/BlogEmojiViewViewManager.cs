using System;
using System.Collections.Generic;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BlogEmojiViewViewManager: IBlogEmojiViewViewService
    {
        private IBlogEmojiViewViewDal _blogEmojiViewDal;

        public BlogEmojiViewViewManager(IBlogEmojiViewViewDal blogEmojiViewDal)
        {
            _blogEmojiViewDal = blogEmojiViewDal;
        }

        public IDataResult<List<BlogEmojiViewView>> GetByBlogId(int blogId)
        {
            return new SuccessDataResult<List<BlogEmojiViewView>>(_blogEmojiViewDal.GetList(i => i.Id == blogId));
        }

        public IDataResult<List<BlogEmojiViewView>> GetByUserId(int userId)
        {
            return new SuccessDataResult<List<BlogEmojiViewView>>(_blogEmojiViewDal.GetList(i => i.UserId == userId));
        }

        public IDataResult<List<BlogEmojiViewView>> GetList()
        {
            return new SuccessDataResult<List<BlogEmojiViewView>>(_blogEmojiViewDal.GetList());
        }

        public IResult UserViewed(int blogId, int userId)
        {
            try
            {
                var u = _blogEmojiViewDal.Get(i => i.UserId == userId && i.Id == blogId);

                return new SuccessResult(message:u.ToString());
            }
            catch (Exception e)
            {
                return new ErrorResult();
            }
            //if (u == null)
            //{
            //    return new ErrorResult();
            //}
            //return new SuccessResult();
        }
    }
}