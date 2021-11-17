using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Filters;
using Business.Abstract;
using Business.Constants;
using DataAccess.Concrete.Filter;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomModel]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IWebHostEnvironment _webHostEnvironment;
        private IUserNotificationService _userNotificationService;
        private IBlogEmojiService _blogEmojiService;
        private IBlogCommentService _blogCommentService;
        private IBlogService _blogService;

        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment, IUserNotificationService userNotificationService, IBlogEmojiService blogEmojiService, IBlogCommentService blogCommentService, IBlogService blogService)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _userNotificationService = userNotificationService;
            _blogEmojiService = blogEmojiService;
            _blogCommentService = blogCommentService;
            _blogService = blogService;
        }

        [HttpGet("getUser")]
        [Authorize]
        public IActionResult getUser(int id)
        {
            //var me = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.System).Data;
            var user = _userService.GetById(id, Status.Per.User);
            if (user.Success)
            {
                return Ok(user.Data.ToDetail());
            }

            return BadRequest(Messages.UserNotFound);
        }

        [HttpGet("getUserReaded")]
        [Authorize]
        public IActionResult getUserReaded(int id, int pageId, int pageSize = 20)
        {
            if (pageSize > 40)
                pageSize = 40;
            if (pageSize < 1)
                pageSize = 1;
            --pageId;

            var pageFiliter = new BlogPageFilter();
            pageFiliter.PageNumber = pageId;
            pageFiliter.PageSize = pageSize;
            //var me = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.System).Data;
            var user = _userService.GetById(id, Status.Per.User);
            if (!user.Success)
            {
                return BadRequest(Messages.UserNotFound);
            }

            //var blogEmoji = _blogEmojiService.GetByUserIdAndEmojiId(user.Data.Id, 1).Data.OrderByDescending(be => be.Id).Select(be => be.BlogId).Skip(pageId * pageSize).Take(pageSize);
            //var blogs = blogEmoji.Select(be => _blogService.GetById(be).Data);

            var blogs = _blogService.GetByUserReaded(user.Data.Id, pageFiliter);
            if (!blogs.Success)
            {
                return BadRequest(Messages.BlogNotFound);
            }

            return Ok(blogs.Data);
        }

        [HttpGet("getUserBlog")]
        [Authorize]
        public IActionResult getUserBlog(int id, int pageId, int pageSize = 20)
        {
            if (pageSize > 40)
                pageSize = 40;
            if (pageSize < 1)
                pageSize = 1;
            --pageId;

            var pageFiliter = new BlogPageFilter();
            pageFiliter.PageNumber = pageId;
            pageFiliter.PageSize = pageSize;
            //var me = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.System).Data;
            var user = _userService.GetById(id, Status.Per.User);
            if (!user.Success)
            {
                return BadRequest(Messages.UserNotFound);
            }


            //var blogComment = _blogService.GetByAuthorId(pageFiliter, user.Data.Id, Status.Per.User).Data.OrderByDescending(b => b.Id).Skip(pageId * pageSize).Take(pageSize);
            //var blogs = _blogService.GetByAuthorId(pageFiliter, user.Data.Id, Status.Per.User).Data.Select(b => b.ToSummary(user.Data.Nickname));
            var blogs = _blogService.GetByAuthorId(user.Data.Id, pageFiliter, Status.Per.User).Data;
            //.Select(b => b.ToSummary(user.Data.Nickname)).ToList();

            return Ok(blogs);
        }

        [HttpGet("getUserByName")]
        [Authorize]
        public IActionResult getUserByName(string name)
        {
            //var me = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.System).Data;
            var user = _userService.GetByNickname(name, Status.Per.User);
            if (user.Success)
            {
                return Ok(user.Data.ToDetail());
            }

            return BadRequest(Messages.UserNotFound);
        }

        [HttpGet("getMyProfil")]
        [Authorize]
        public IActionResult getMyProfil()
        {
            var user = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),Status.Per.Me).Data;
            //UserDetailsDto userDetailsDto = new UserDetailsDto
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    Nickname = user.Nickname,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Description = user.Description,
            //    AvatarUrl = user.AvatarUrl
            //};
            return Ok(user.ToDetail());
        }

        [HttpPost("updateUser")]
        [Authorize]
        public IActionResult updateUser(UserDetailsDto userPost)
        {
            var result = _userService.Update(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), userPost,Status.Per.Me);

            if (!result.Success)
            {
                //return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message});
                return BadRequest(result);
            }

            return Ok(userPost);
        }

        [Authorize]
        [HttpPost("updateUserPp"), DisableRequestSizeLimit]
        public IActionResult UpdateUserPp([FromForm] IFormFile objectFile)
        {
            var user = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),Status.Per.Me);
            if (user.Data == null)
            {
                return BadRequest();
            }
            if (((Status.Neno ^ user.Data.Status) & Status.Sban[0]) != 0)//Profil banı var mı?
            {
                return BadRequest(Messages.UserUpdateBan);
            }
            try
            {
                if (objectFile.Length > 0/* && formFile.Length < 21000000*/)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "images")))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var fileName = RandomString(4) + user.Data.Id + ".jpg";
                    var fullPath = Path.Combine(path, fileName);

                    if (user.Data.AvatarUrl != null)
                    {
                        if (System.IO.File.Exists(Path.Combine(path, user.Data.AvatarUrl)))
                        {
                            System.IO.File.Delete(Path.Combine(path, user.Data.AvatarUrl));
                        }
                    }

                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        objectFile.CopyTo(fileStream);
                        fileStream.Flush();
                        _userService.UpdatePp(user.Data.Id, fileName);

                        return Ok(fileName);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("setOneSignalId")]
        [Authorize]
        public IActionResult SetOneSignalId(string oneSignalId)
        {
            var user = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.Me);

            if (!user.Success)
            {
                return BadRequest(user);
            }

            _userService.UpdateOneSignalId(user.Data.Id, oneSignalId);

            return Ok();
        }

        [HttpGet("getNotifications")]
        [Authorize]
        public IActionResult getNotifications()
        {
            var user = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.Me);

            if (!user.Success)
            {
                return BadRequest(user);
            }
            //user.Data.Status

            var userNotifications = _userNotificationService.GetByUserId(user.Data.Id);
            if (!userNotifications.Success)
            {
                return BadRequest(userNotifications);
            }

            var result = new List<UserNotification>(userNotifications.Data);


            var notReadedUserNotificationsId = (from userNotification in userNotifications.Data    //burası görüldü atar
                where userNotification.Readed == false
                select userNotification.Id).ToList();


            for (int i = 0; i < notReadedUserNotificationsId.Count; i++)
            {
                var update = _userNotificationService.SetReaded(notReadedUserNotificationsId[i]);
                if (!update.Success)
                {
                    return BadRequest(update);
                }
            }



            return Ok(result);
        }

        //
        //[HttpGet("changeSettings")]
        //[Authorize]
        //public IActionResult changeSettings()
        //{
        //    var user = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)), Status.Per.Me);

        //    if (!user.Success)
        //    {
        //        return BadRequest(user);
        //    }
        //    //user.Data.Status

        //    return Ok();
        //}

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
