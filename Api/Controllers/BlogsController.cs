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

////blog detail i ceker
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
        private IAuthorService _authorService;
        private IUserNotificationService _userNotificationService;
        private IDateTimeHelper _dateTimeHelper;

        public BlogsController(IBlogService blogService, IBlogCommentService blogCommentService, IUserService userService, IBlogEmojiViewService blogEmojiViewService, IBlogEmojiService blogEmojiService, IAuthorService authorService, IDateTimeHelper dateTimeHelper, IUserNotificationService userNotificationService)
        {
            _blogService = blogService;
            _blogCommentService = blogCommentService;
            _userService = userService;
            _blogEmojiViewService = blogEmojiViewService;
            _blogEmojiService = blogEmojiService;
            _authorService = authorService;
            _dateTimeHelper = dateTimeHelper;
            _userNotificationService = userNotificationService;
        }

        //blog detail i ceker
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

        ////blog detail i ceker
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

        //blog detail i ceker
        [HttpGet("getBlog")]
        [Authorize]
        public IActionResult GetBlog(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(id, Status.Per.User);

            if (blog.Success)
            {
                _blogService.GetBlogView(blog.Data.Id, userId);
                return Ok(blog.Data.ToDetail(_authorService.GetById(blog.Data.AuthorId), _blogService.GetTags(blog.Data.Id)));
            }

            return BadRequest();
        }

        //blog detail i ceker
        [HttpGet("getBlogGuest")]
        public IActionResult GetBlogGuest(int id)
        {
            var blog = _blogService.GetById(id, Status.Per.User);

            if (blog.Success)
            {
                return Ok(blog.Data.ToDetail(_authorService.GetById(blog.Data.AuthorId), _blogService.GetTags(blog.Data.Id)));
            }

            return BadRequest();
        }

        //blog detail i ceker
        [HttpGet("getWebBlog")]
        [Authorize]
        public ContentResult GetWebBlog(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var blog = _blogService.GetById(id, Status.Per.User);

            var result = _blogService.GetHtmlBlog(blog.Data.ToDetail(_authorService.GetById(blog.Data.AuthorId), _blogService.GetTags(blog.Data.Id)), userId);

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

        //blog detail i ceker
        [HttpGet("getWebBlogGuest")]
        public ContentResult GetWebBlogGuest(int id)
        {
            var blog = _blogService.GetById(id, Status.Per.User);
            var result = _blogService.GetHtmlBlog(blog.Data.ToDetail(_authorService.GetById(blog.Data.AuthorId), _blogService.GetTags(blog.Data.Id)));

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
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //object metadata;
            var result = _blogService.GetSearchList(text);

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            result.Data.Reverse();
            return Ok(result.Data);
        }

        //blog detail i ceker
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
                UserId = mainComment.Data.UserId,
                CommentId = mainComment.Data.Id,
                CommentDate = _dateTimeHelper.SetTime(mainComment.Data.CommentDate),
                CommentResponse = 0,
                Text = mainComment.Data.Text,
                UserAvatarUrl = _userService.GetById(mainComment.Data.UserId,Status.Per.System).Data.ToDetail().AvatarUrl,
                UserNickname = _userService.GetById(mainComment.Data.UserId, Status.Per.System).Data.ToDetail().Nickname
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

                var author = _authorService.GetById(blog.Data.AuthorId);
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
                userNotifications.UserId = author.Data.UserId;
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