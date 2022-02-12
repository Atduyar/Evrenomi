using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Filters;
using Business.Abstract;
using Business.Constants;
using Business.Helpers;
using Core.Utilities.Results;
using DataAccess.Concrete.Filter;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

//
////Blogları listeler

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomModel]
    public class BlogsController : ControllerBase
    {
        private IBlogService _blogService;
        private IBlogCommentService _blogCommentService;
        private IBlogEmojiViewService _blogEmojiViewService;
        private IBlogEmojiService _blogEmojiService;
        private IUserService _userService;
        private IUserNotificationService _userNotificationService;
        private IDateTimeHelper _dateTimeHelper;
        private IBlogTagService _blogTagService;
        private ITagService _tagService;

        public BlogsController(IBlogService blogService, IBlogCommentService blogCommentService, IUserService userService, IBlogEmojiViewService blogEmojiViewService, IBlogEmojiService blogEmojiService, IDateTimeHelper dateTimeHelper, IUserNotificationService userNotificationService, IBlogTagService blogTagService, ITagService tagService)
        {
            _blogService = blogService;
            _blogCommentService = blogCommentService;
            _userService = userService;
            _blogEmojiViewService = blogEmojiViewService;
            _blogEmojiService = blogEmojiService;
            _dateTimeHelper = dateTimeHelper;
            _userNotificationService = userNotificationService;
            _blogTagService = blogTagService;
            _tagService = tagService;
        }

        [HttpGet("addBlog")]
        [Authorize]
        public IActionResult addBlog(BlogDetailDto blogDetailDto)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.Add(new Blog
            {
                AuthorId = userId,
                BlogTitle = blogDetailDto.BlogTitle,
                BlogSummary = blogDetailDto.BlogSummary,
                BlogTitlePhotoUrl = blogDetailDto.BlogTitlePhotoUrl,
                BlogDate = DateTime.Now, //blogDetailDto.BlogDate,
                BlogContent = JsonConvert.SerializeObject(blogDetailDto.BlogContent), //blogDetailDto.BlogContent,
                BlogStatus = Status.Approval
            });

            if (!blog.Success)
            {
                return BadRequest(blog);
            }

            if (blog.Success)
            {
                List<Tag> tagNames = blogDetailDto.BlogTags;
                for (int i = 0; i < tagNames.Count; i++)
                {
                    var tag = _tagService.GetById(tagNames[i].Id);
                    if (!tag.Success)
                    {
                        continue;
                    }
                    _blogTagService.Add(new BlogTag
                    {
                        BlogId = blog.Data,
                        TagId = tag.Data.Id,
                    });
                }
            }


            return Ok(blog);
        }

        [HttpGet("addBlogDraft")]
        [Authorize]
        public IActionResult addBlogDraft(string blogTitle)
        {
            if (string.IsNullOrWhiteSpace(blogTitle))
            {
                return BadRequest(Messages.BlogTitleEmpty);
            }
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.Add(new Blog
            {
                AuthorId = userId,
                BlogTitle = blogTitle,
                BlogSummary = "-",
                BlogContent = "[]",
                BlogDate = DateTime.Now, //blogDetailDto.BlogDate,
                BlogStatus = Status.DefBlog | Status.Hidden
            });

            if (!blog.Success)
            {
                return BadRequest(blog);
            }

            return Ok(blog);
        }
        [HttpPost("updateBlog")]
        [Authorize]
        public IActionResult updateBlog(BlogDetailDto blogDetailDto)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(blogDetailDto.BlogId, Status.Per.System);

            if (!blog.Success)
            {
                return BadRequest(blog);
            }

            if (userId != blog.Data.AuthorId)
            {
                return BadRequest(Messages.BlogNotAccessible);
            }

            blog.Data.BlogTitle = blogDetailDto.BlogTitle;
            blog.Data.BlogTitlePhotoUrl = blogDetailDto.BlogTitlePhotoUrl;
            blog.Data.BlogSummary = blogDetailDto.BlogSummary;
            blog.Data.BlogContent = JsonConvert.SerializeObject(blogDetailDto.BlogContent);
            blog.Data.BlogDate = DateTime.Now;

            var blogU = _blogService.Update(blog.Data);

            if (!blogU.Success)
            {
                return BadRequest(blogU);
            }

            var tags = _blogTagService.GetByBlogId(blog.Data.Id);
            if (!tags.Success)
            {
                return BadRequest(tags);
            }

            var blogTags = tags.Data.Select(bt => bt.TagId);
            //return Ok(_tagService.GetByName("Robot"));
            var getNewTags = blogDetailDto.BlogTags.Select(bt => _tagService.GetByName(bt.Name)).Where(t => t.Success && t.Data != null).Select(td => td.Data);
            //return Ok(getNewTags);
            var blogTagsNew = getNewTags.Select(bt => bt.Id);
            //return Ok(blogTagsNew);
            var removeList = blogTags.Except(blogTagsNew).ToList();
            var addList = blogTagsNew.Except(blogTags).ToList();
            //return Ok(removeList);
            for (int i = 0; i < removeList.Count(); i++)
            {
                var deleteTag = _blogTagService.GetByBlogIdAndTagId(blog.Data.Id, removeList[i]);
                if (deleteTag.Success)
                {
                    _blogTagService.Delete(deleteTag.Data);
                }
            }
            for (int i = 0; i < addList.Count(); i++)
            {
                _blogTagService.Add(new BlogTag() { BlogId = blog.Data.Id, TagId = addList[i] });
            }
            //if (blogU.Success)
            //{
            //    List<Tag> tagNames = blogDetailDto.BlogTags;
            //    var tagNamesOld = _blogTagService.GetByBlogId(blog.Data.Id);
            //    if (!tagNamesOld.Success)
            //    {
            //        return BadRequest(tagNamesOld);
            //    }

            //    for (int i = 0; i < tagNames.Count; i++)
            //    {
            //        ////////////////////////////////////////tag varsa ekle sil ayarla iste
            //        var tag = _tagService.GetById(tagNames[i].Id);
            //        if (!tag.Success)
            //        {
            //            continue;
            //        }
            //        _blogTagService.Add(new BlogTag
            //        {
            //            BlogId = blog.Data.Id,
            //            TagId = tag.Data.Id,
            //        });
            //    }
            //}

            return Ok(blog.Data.ToDetail(_userService.GetById(blog.Data.AuthorId, Status.Per.System), _blogService.GetTags(blog.Data.Id)));
        }
        [HttpGet("publishBlog")]
        [Authorize]
        public IActionResult publishBlogDraft(int blogId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(blogId, Status.Per.System);
            if (!blog.Success)
            {
                return BadRequest(blog);
            }

            if (blog.Data.AuthorId != userId)
            {
                return BadRequest(Messages.BlogNotAccessible);
            }

            if ((blog.Data.BlogStatus & Status.Hidden) == 0)//taslak değil se
            {
                return BadRequest(Messages.BlogAlreadyPublish);
            }

            var blogU = _blogService.UpdateStatus(blog.Data.Id, blog.Data.BlogStatus - Status.Hidden, Status.Per.System);// Status.Per.Me olarak ayarla

            if (!blogU.Success)
            {
                return BadRequest(blogU);
            }


            return Ok(blogU);
        }

        [HttpGet("unPublishBlog")]
        [Authorize]
        public IActionResult unPublishBlogDraft(int blogId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(blogId, Status.Per.System);
            if (!blog.Success)
            {
                return BadRequest(blog);
            }

            if (blog.Data.AuthorId != userId)
            {
                return BadRequest(Messages.BlogNotAccessible);
            }

            if ((blog.Data.BlogStatus & Status.Hidden) == Status.Hidden)//taslak değil se
            {
                return BadRequest(Messages.BlogAlreadyPublish);
            }

            var blogU = _blogService.UpdateStatus(blog.Data.Id, blog.Data.BlogStatus + Status.Hidden, Status.Per.System);// Status.Per.Me olarak ayarla

            if (!blogU.Success)
            {
                return BadRequest(blogU);
            }


            return Ok(blogU);
        }

        [HttpGet("getBlogDraft")]
        [Authorize]
        public IActionResult GetBlogDraft(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(id, Status.Per.System);

            if (!blog.Success)
            {
                return BadRequest(blog);
            }
            if (userId != blog.Data.AuthorId)
            {
                return BadRequest(Messages.BlogNotAccessible);
            }

            return Ok(blog.Data.ToDetail(_userService.GetById(blog.Data.AuthorId, Status.Per.System), _blogService.GetTags(blog.Data.Id)));
        }
        [HttpGet("getMyBlogs")]
        [Authorize]
        public IActionResult getMyBlogs(int pageId, int pageSize = 20)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (pageSize > 40)
                pageSize = 40;
            if (pageSize < 1)
                pageSize = 1;
            --pageId;

            var pageFiliter = new BlogPageFilter();
            pageFiliter.PageNumber = pageId;
            pageFiliter.PageSize = pageSize;

            //var blogs = _blogService.getUserBlog(userId, Status.Per.System);
            var blogs = _blogService.GetByAuthorId(userId, pageFiliter, Status.Per.System);
            if (!blogs.Success)
            {
                return BadRequest(blogs);
            }

            return Ok(blogs.Data);
        }

        [HttpGet("deleteBlog")]
        [Authorize]
        public IActionResult deleteBlog(int blogId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(blogId, Status.Per.System);
            if (!blog.Success)
            {
                return BadRequest(blog);
            }

            if (blog.Data.AuthorId != userId)
            {
                return BadRequest(Messages.BlogNotAccessible);
            }

            var result = _blogService.Delete(blog.Data);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            var resultTag = _blogTagService.GetByBlogId(blog.Data.Id);
            for (int i = 0; i < resultTag.Data.Count; i++)
            {
                _blogTagService.Delete(resultTag.Data[i]);
            }

            return Ok(result);
        }
        
        //[HttpGet("getBlogView")]
        //[Authorize]
        //public IActionResult GetBlogView(int id)
        //{
        //    var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //    var blog = _blogService.GetById(id, Status.Per.User);

        //    if (blog.Success)
        //    {
        //        var bv = _blogService.GetBlogViewDto(blog.Data);
        //        if (bv.Success)
        //        {
        //            _blogService.GetBlogView(blog.Data.Id, userId);
        //            return Ok(bv.Data);
        //        }
        //    }

        //    return BadRequest();
        //}

        //
        //[HttpGet("getBlogViewGuest")]
        //public IActionResult GetBlogViewGuest(int id)
        //{
        //    var blog = _blogService.GetById(id, Status.Per.User);

        //    if (blog.Success)
        //    {
        //        var bv = _blogService.GetBlogViewDto(blog.Data);
        //        if (bv.Success)
        //        {
        //            return Ok(bv.Data);
        //        }
        //    }

        //    return BadRequest();
        //}

        
        [HttpGet("getBlogMeta")]
        public IActionResult getBlogMeta(int id)
        {
            var blog = _blogService.GetById(id, Status.Per.User);

            if (blog.Success)
            {
                return Ok(new BlogMetaDto()
                {
                    BlogTitle = blog.Data.BlogTitle,
                    BlogSummary = blog.Data.BlogSummary,
                    BlogTitlePhotoUrl = blog.Data.BlogTitlePhotoUrl
                });
            }

            return BadRequest();
        }

        
        [HttpGet("getBlog")]
        [Authorize]
        public IActionResult GetBlog(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(id, Status.Per.User);

            if (blog.Success)
            {
                _blogService.GetBlogView(blog.Data.Id, userId);
                return Ok(blog.Data.ToDetail(_userService.GetById(blog.Data.AuthorId, Status.Per.User), _blogService.GetTags(blog.Data.Id)));
            }

            return BadRequest();
        }

        [HttpGet("getBlogGuest")]
        public IActionResult GetBlogGuest(int id)
        {
            var blog = _blogService.GetById(id, Status.Per.User);

            if (blog.Success)
            {
                return Ok(blog.Data.ToDetail(_userService.GetById(blog.Data.AuthorId,Status.Per.UnUser), _blogService.GetTags(blog.Data.Id)));
            }

            return BadRequest();
        }

        
        [HttpGet("getWebBlog")]
        [Authorize]
        public ContentResult GetWebBlog(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(id, Status.Per.User);

            var result = _blogService.GetHtmlBlog(blog.Data.ToDetail(_userService.GetById(blog.Data.AuthorId,Status.Per.User), _blogService.GetTags(blog.Data.Id)), userId);

            if (result.Success)
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = result.Data
                };
            }
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        
        [HttpGet("getWebBlogGuest")]
        public ContentResult GetWebBlogGuest(int id)
        {
            var blog = _blogService.GetById(id, Status.Per.User);
            var result = _blogService.GetHtmlBlog(blog.Data.ToDetail(_userService.GetById(blog.Data.AuthorId, Status.Per.UnUser), _blogService.GetTags(blog.Data.Id)));

            if (result.Success)
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = result.Data
                };
            }
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        //Blogları listeler
        [HttpPost("getByPageGuest")]
        public IActionResult GetByPageGuest(BlogPageFilter blogPageFilter)
        {
            object metadata;
            var result = _blogService.GetPage(blogPageFilter, out metadata,Status.Per.User);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            result.Data.Reverse();
            return Ok(result.Data);
            //var result = _blogService.GetList();
            //if (result.Success)
            //{
            //    return Ok(result.Data);
            //}
            //return BadRequest(result.Message);
        }

        //Blogları listeler
        [HttpPost("getByPage")]
        [Authorize]
        public IActionResult GetByPage(BlogPageFilter blogPageFilter)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            object metadata;
            var result = _blogService.GetPage(blogPageFilter, userId, out metadata, Status.Per.User);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            result.Data.Reverse();
            return Ok(result.Data);
            //var result = _blogService.GetList();
            //if (result.Success)
            //{
            //    return Ok(result.Data);
            //}
            //return BadRequest(result.Message);
        }

        //Blog arar
        [HttpPost("searchBlogs")]
        [Authorize]
        public IActionResult searchBlogs(List<string> text)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //object metadata;
            var result = _blogService.GetSearchList(text);

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            result.Data.Reverse();
            return Ok(result.Data);
        }
        //Blog arar
        [HttpPost("searchBlogsGuest")]
        public IActionResult searchBlogsGuest(List<string> text)
        {
            //object metadata;
            var result = _blogService.GetSearchList(text);

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            result.Data.Reverse();
            return Ok(result.Data);
        }

        
        [HttpGet("getBlogComment")]
        public IActionResult GetBlogComment(int blogId)
        {
            var comments = _blogCommentService.GetByCommentForBlog(blogId);
            if (!comments.Success)
            {
                return BadRequest(comments);
                //return BadRequest(comments.Message);
            }

            comments.Data.Reverse();
            return Ok(comments.Data);
        }

        [HttpGet("getBlogCommentResponse")]
        public IActionResult GetBlogCommentResponse(int blogCommentId)
        {
            var commentResponses = _blogCommentService.GetByCommentResponseForBlog(blogCommentId);
            if (!commentResponses.Success)
            {
                return BadRequest(commentResponses);
                //return BadRequest(commentResponses.Message);
            }

            var mainComment = _blogCommentService.GetById(blogCommentId);

            if (!mainComment.Success && mainComment.Data != null)
            {
                return BadRequest();
            }
            commentResponses.Data.Insert(0, new CommentForBlog()
            {
                BlogId = mainComment.Data.BlogId,
                CommentId = mainComment.Data.Id,
                CommentDate = _dateTimeHelper.SetTime(mainComment.Data.CommentDate),
                CommentResponse = 0,
                Text = mainComment.Data.Text,

                UserSummary = new UserSummaryDto() { Id = mainComment.Data.UserId, Nickname = _userService.GetById(mainComment.Data.UserId, Status.Per.System).Data.ToDetail().Nickname, AvatarUrl = _userService.GetById(mainComment.Data.UserId, Status.Per.System).Data.ToDetail().AvatarUrl }
            });
            //commentResponses.Data.Reverse();
            return Ok(commentResponses.Data);
        }

        [HttpPost("addBlogComment")]
        [Authorize]
        public IActionResult AddBlogComment(AddCommentForBlog addCommentForBlog)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _userService.GetById(userId,Status.Per.Me);
            if (!user.Success)
            {
                return BadRequest(user);
            }
            var userNotifications = new UserNotification()
            {
                Sender = userId.ToString(),
                Readed = false
            };


            IDataResult<Blog> blog = null;
            IDataResult<int> result = null;
            if (addCommentForBlog.ParentBlogCommentId == null) //commentResponse
            {
                blog = _blogService.GetById(addCommentForBlog.BlogId, Status.Per.System);
                if (!blog.Success)
                {
                    return BadRequest(blog);
                }
            }

            if (addCommentForBlog.ParentBlogCommentId != null)//commentResponse
            {
                var comment = _blogCommentService.GetByCommentIdForBlog(Convert.ToInt32(addCommentForBlog.ParentBlogCommentId));
                if (!comment.Success)
                {
                    return BadRequest(comment);
                }

                var parentCommet = _blogCommentService.GetById(comment.Data.CommentId);
                if (!parentCommet.Success)
                {
                    return BadRequest(parentCommet);
                }
                addCommentForBlog.BlogId = parentCommet.Data.BlogId;

                blog = _blogService.GetById(addCommentForBlog.BlogId, Status.Per.System);
                if (!blog.Success)
                {
                    return BadRequest(blog);
                }



                result = _blogCommentService.AddCommentForBlog(addCommentForBlog, userId);
                if (!result.Success)
                {
                    return BadRequest(result);
                    //return BadRequest(result.Message);
                }



                userNotifications.Header = "1 yeni yanıt";
                userNotifications.UserId = parentCommet.Data.UserId;
                userNotifications.IconUrl = "Images/" + user.Data.AvatarUrl;
                userNotifications.Message = user.Data.Nickname + " yorumunu yanıtladı: " + (addCommentForBlog.Text.Length > 200
                    ? addCommentForBlog.Text.Substring(0, 200)
                    : addCommentForBlog.Text);
                userNotifications.Data = "{\"type\":\"blogCommentResponse\",\"blogId\":" + addCommentForBlog.BlogId + ",\"commentId\":" + addCommentForBlog.ParentBlogCommentId + ",\"commentResponse\":" + result.Data.ToString() + "}";
            }
            else//yazara mail ayarlar
            {

                var author = _userService.GetById(blog.Data.AuthorId, Status.Per.System);
                if (!author.Success)
                {
                    return BadRequest(author);
                }



                result = _blogCommentService.AddCommentForBlog(addCommentForBlog, userId);
                if (!result.Success)
                {
                    return BadRequest(result);
                }




                userNotifications.Header = "1 yeni yorum";
                userNotifications.UserId = author.Data.Id;
                userNotifications.IconUrl = "Images/" + user.Data.AvatarUrl;
                userNotifications.Message = user.Data.Nickname + ": " + (addCommentForBlog.Text.Length > 200
                    ? addCommentForBlog.Text.Substring(0, 200)
                    : addCommentForBlog.Text);
                userNotifications.Data = "{\"type\":\"blogComment\",\"blogId\":" + addCommentForBlog.BlogId + ",\"commentId\":" + result.Data.ToString() + "}";
            }

            var toPerson = _userService.GetById(userNotifications.UserId, Status.Per.System);
            if (!toPerson.Success)
            {
                return BadRequest(toPerson);
            }

            if (toPerson.Data.Id != userId)//toPerson.Data.Status > 0
            {
                var userNResult = _userNotificationService.Add(userNotifications);
                if (!userNResult.Success)
                {
                    BadRequest(userNResult);
                }

                var sendMes = _userNotificationService.SendNotifications(toPerson.Data.OneSignalId, userNotifications.Header, userNotifications.Message, userNotifications.Data);
                if (!sendMes.Success)
                {
                    return BadRequest(sendMes);
                }
            }
            return Ok(result);
        }

        [HttpPost("updateBlogComment")]
        [Authorize]
        public IActionResult UpdateBlogComment(CommentForBlog commentForBlog)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var comment = _blogCommentService.GetById(commentForBlog.CommentId);
            if (comment.Data == null || !comment.Success)
            {
                return BadRequest(Messages.BlogCommentNotFound);
            }
            if (comment.Data.UserId != userId)
            {
                return BadRequest(Messages.BlogCommentNotAccessible);
            }

            var blogComment = _blogCommentService.Update(new BlogComment
            {
                Id = comment.Data.Id,
                BlogId = comment.Data.BlogId,
                UserId = userId,
                Text = commentForBlog.Text,
                CommentDate = DateTime.Now,
                Status = comment.Data.Status
            });
            if (!blogComment.Success)
            {
                return BadRequest(blogComment);
                //return BadRequest(blog.Message);
            }

            return Ok(blogComment);
        }

        [HttpPost("deleteBlogComment")]
        [Authorize]
        public IActionResult DeleteBlogComment(CommentForBlog commentForBlog)
        {
            var user = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),Status.Per.System);
            if (user.Data == null || !user.Success)
            {
                return BadRequest(Messages.UserNotFound);
            }
            var comment = _blogCommentService.GetById(commentForBlog.CommentId);
            if (comment.Data.UserId != user.Data.Id)
            {
                return BadRequest(Messages.BlogCommentNotAccessible);
            }

            var result = _blogCommentService.Delete(comment.Data);
            if (!result.Success)
            {
                return BadRequest(result);
                //return BadRequest(result.Message);
            }

            return Ok(result);
        }

        //Kullanıcı görüntüle
        [HttpGet("getUserComment")]
        [Authorize]
        public IActionResult getUserComment(int id, int pageId, int pageSize = 20)
        {
            if (pageSize > 40)
                pageSize = 40;
            if (pageSize < 1)
                pageSize = 1;
            --pageId;
            //var me = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.System).Data;
            var user = _userService.GetById(id, Status.Per.User);
            if (!user.Success)
            {
                return BadRequest(Messages.UserNotFound);
            }

            var blogComment = _blogCommentService.GetByUserId(user.Data.Id).Data.OrderByDescending(bc => bc.Id).Skip(pageId * pageSize).Take(pageSize);

            var result = from c in blogComment
                         select new CommentForBlog
                         {
                             CommentId = c.Id,
                             BlogId = c.BlogId,
                             CommentDate = _dateTimeHelper.SetTime(c.CommentDate),
                             CommentResponse = _blogCommentService.GetByCommentResponse(c.Id).Data.Count,
                             Text = c.Text,
                             UserSummary = new UserSummaryDto() { Id = c.UserId, Nickname = _userService.GetById(c.UserId, Status.Per.System).Data.ToDetail().Nickname, AvatarUrl = _userService.GetById(c.UserId, Status.Per.System).Data.ToDetail().AvatarUrl }
                         };
            return Ok(result);
        }

        [HttpPost("giveEmoji")]
        [Authorize]
        public IActionResult GiveEmoji(int blogId, int emojiId)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var bev = _blogEmojiViewService.UserViewed(blogId, userId, emojiId);
            if (bev.Success)// zaten verildiyse değistiriyor
            {
                var be =_blogEmojiService.GetByEmojiView(bev.Data);
                be.Data.EmojiId = emojiId;
                return Ok(_blogEmojiService.Update(be.Data));
            }

            return Ok(_blogEmojiService.Add(new BlogEmoji
            {
                BlogId = blogId,
                UserId = userId,
                EmojiId = emojiId
            }));
        }
    }

}

/*
GET	    /blogs/getBlog	                    Auth    HtmlPage	            ?id=(int)
GET	    /blogs/getBlogGuest                    	    HtmlPage	            ?id=(int)
GET	    /blogs/getWebBlog	                Auth    BlogDetailDto	        ?id=(int)
GET	    /blogs/getWebBlogGuest                 	    BlogDetailDto           ?id=(int)
GET	    /blogs/getBlogComment	                	List<CommentForBlog>	?blogId=(int)
GET	    /blogs/getBlogCommentResponse	        	List<CommentForBlog>    ?blogCommentId=(int)
POST	/blogs/getbypage	            	        List<BlogSummaryDto>    BlogPageFilter
POST	/blogs/addBlogComment               Auth	OK	                    AddCommentForBlog
POST	/blogs/updateBlogComment	        Auth	Ok	                    CommentForBlog
POST	/blogs/deleteBlogComment            Auth	OK	                    CommentForBlog
*/