using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Filters;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    [CustomModel]
    public class AdminController : ControllerBase
    {
        private IUserService _userService;
        private IOperationClaimService _operationClaimService;
        private IUserOperationClaimService _userOperationClaimService;
        private IAuthorService _authorService;
        private IBlogService _blogService;
        private IBlogTagService _blogTagService;

        public AdminController(IUserService userService, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService, IAuthorService authorService, IBlogService blogService, IBlogTagService blogTagService)
        {
            _userService = userService;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _authorService = authorService;
            _blogService = blogService;
            _blogTagService = blogTagService;
        }

        //Kullanıcı cekme
        [HttpGet("getUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUser(int userId)
        {
            var user = _userService.GetById(userId,Status.Per.Admin).Data;
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
            return Ok(user);
        }

        [HttpGet("getBlog")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBlog(int blogId)
        {
            var blog = _blogService.GetById(blogId,Status.Per.Admin).Data;

            //BlogDetailDto blogDetailsDto = new BlogDetailDto
            //{
            //    BlogId = blog.Id,
            //    BlogTitle = blog.BlogTitle,
            //    AuthorId = blog.AuthorId,
            //    BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
            //    BlogContent = blog.BlogContent,
            //    BlogDate = blog.BlogDate,
            //    BlogTags = String.Join(",", _blogService.GetTags(blogId).Data.Select(t => t.Name)),
            //    BlogSummary = blog.BlogSummary
            //};
            //return Ok(blogDetailsDto);
            return Ok(blog);
        }

        [HttpGet("getAuthor")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAuthor(int authorId)
        {
            var author = _authorService.GetById(authorId/*, Status.Per.Admin*/).Data;

            //BlogDetailDto blogDetailsDto = new BlogDetailDto
            //{
            //    BlogId = blog.Id,
            //    BlogTitle = blog.BlogTitle,
            //    AuthorId = blog.AuthorId,
            //    BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl,
            //    BlogContent = blog.BlogContent,
            //    BlogDate = blog.BlogDate,
            //    BlogTags = String.Join(",", _blogService.GetTags(blogId).Data.Select(t => t.Name)),
            //    BlogSummary = blog.BlogSummary
            //};
            //return Ok(blogDetailsDto);
            return Ok(author);
        }

        [HttpPost("getBlogView")]
        [Authorize(Roles = "Admin,Author")]
        public ContentResult GetBlogView(BlogDetailDto blogDetailDto)
        {
            var result = _blogService.GetHtmlBlog(blogDetailDto);

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
        

        //Tüm kullanıcıları cekme
        [HttpGet("getAllUserSummary")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUserSummary()
        {
            var user = _userService.GetListSummary(Status.Per.Admin).Data;
            return Ok(user);
        }

        //Tüm kullanıcıları cekme
        [HttpGet("getAllUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUser()
        {
            var user = _userService.GetList(Status.Per.Admin).Data;
            return Ok(user);
        }

        //Tüm Blogları ceker
        [HttpGet("getAllBlog")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBlog()
        {
            var result = _blogService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        //Yazar ekleme
        [HttpPost("addAuthor")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAuthor(AuthorForRegister authorForRegister)
        {

            var author = _authorService.Add(new Author
            {
                UserId = authorForRegister.UserId,
                AuthorName = authorForRegister.AuthorName,
                AuthorDescription = authorForRegister.AuthorDescription,
                AuthorAvatarUrl = authorForRegister.AuthorAvatarUrl,
                AuthorStatus = 1
            });
            if (!author.Success)
            {
                return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = author.Message });
            }
            return Ok(author.Message);
        }

        //Blogları status e göre çeker
        [HttpGet("getAllBlogByStatus")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBlogByStatus(int status)
        {

            var blogs = _blogService.GetByStatus(0,status);///hatalı
            if (!blogs.Success)
            {
                return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = blogs.Message });
            }

            //blogs.Data[0].BlogTags = Convert.ToString(blogs.Data[0].BlogStatus,2);
            return Ok(blogs.Data);
        }

        //Rol Atama
        [HttpPost("setOperationClaimToUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult SetOperationClaimToUser(OperationClaimToUserDto operationClaimToUserDto)
        {
            var user = _userService.GetByEmailOrNickname(operationClaimToUserDto.UserForLoginDto.EmailOrNickname).Data;//user
            var claims = _userService.GerClaims(user).Data;//userin rolleri
            var operationClaim = _operationClaimService.GetById(operationClaimToUserDto.OperationClaim.Id);//gönderilen rol
            if (!operationClaim.Success || operationClaim.Data == null)//söylenen rol var mı?
            {
                return BadRequest(Messages.OperationClaimNotFond);
            }

            var claimControl = (from c in claims
                         where c.Id == operationClaim.Data.Id
                         select c).ToList();

            if (claimControl.Count != 0)//Kullanıcaı zaten bu role sahip mi?
            {
                return BadRequest(Messages.OperationClaimAlreadyExist);
            }

            var result = _userOperationClaimService.Add(new UserOperationClaim
            {
                OperationClaimId = operationClaim.Data.Id,
                UserId = user.Id
            });
            
            return Ok(result.Message);
        }

        //Rol Alma
        [HttpPost("deleteOperationClaimToUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteOperationClaimToUser(OperationClaimToUserDto operationClaimToUserDto)
        {
            var user = _userService.GetByEmailOrNickname(operationClaimToUserDto.UserForLoginDto.EmailOrNickname).Data;//user
            var claims = _userService.GerClaims(user).Data;//userin rolleri
            var operationClaim = _operationClaimService.GetById(operationClaimToUserDto.OperationClaim.Id);//gönderilen rol
            if (!operationClaim.Success || operationClaim.Data == null)//söylenen rol var mı?
            {
                return BadRequest(Messages.OperationClaimNotFond);
            }

            var claimControl = (from c in claims
                where c.Id == operationClaim.Data.Id
                select c).ToList();

            if (claimControl.Count == 0)//Kullanıcaı zaten bu role sahip mi?
            {
                return BadRequest(Messages.OperationClaimNotAvailable);
            }

            var userOperationClaims = _userOperationClaimService.GetByUserId(user.Id);//user in userOperationCleam larını ceker
            var userClaimControl = (from u in userOperationClaims.Data
                where u.OperationClaimId == operationClaim.Data.Id
                select u).ToList();//gönderilen rol user in userOperationCleam larında var mı

            if (userClaimControl.Count == 0)//Kullanıcaı zaten bu role sahip mi?
            {
                return BadRequest(Messages.OperationClaimNotAvailable);
            }

            var result = _userOperationClaimService.Delete(userClaimControl[0]);

            return Ok(result.Message);
        }


        //
        [HttpPost("setUserProfile")]
        [Authorize(Roles = "Admin")]
        public IActionResult setUserProfile(UserDetailsDto userPost)
        {
            var result = _userService.Update(userPost.Id, userPost, Status.Per.System);

            if (!result.Success)
            {
                //return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message});
                return BadRequest(result);
            }

            return Ok(userPost);
        }
        //
        [HttpGet("setUserStatus")]
        [Authorize(Roles = "Admin")]
        public IActionResult setUserStatus(int id, int status)
        {
            var result = _userService.UpdateStatus(id, status, Status.Per.Admin);

            if (!result.Success)
            {
                //return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message});
                return BadRequest(result);
            }

            return Ok(result);
        }

        //
        [HttpGet("setUserPp")]
        [Authorize(Roles = "Admin")]
        public IActionResult setUserPp(int userId)
        {
            var result = _userService.UpdatePp(userId, "defaultPp.png");

            if (!result.Success)
            {
                //return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message});
                return BadRequest(result);
            }

            return Ok(result);
        }

        //
        [HttpGet("setBlogStatus")]
        [Authorize(Roles = "Admin")]
        public IActionResult setBlogStatus(int blogId, int status)
        {
            var result = _blogService.UpdateStatus(blogId, status, Status.Per.Admin);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpPost("setBlogContent")]
        [Authorize(Roles = "Admin")]
        public IActionResult setBlogContent(Blog blog)
        {
            var b = _blogService.GetById(blog.Id);
            if (!b.Success)
            {
                return BadRequest(b.Message);
            }
            b.Data.BlogContent = blog.BlogContent;
            b.Data.BlogSummary = blog.BlogSummary;
            b.Data.BlogTitle = blog.BlogTitle;
            b.Data.BlogTitlePhotoUrl = blog.BlogTitlePhotoUrl;
            b.Data.BlogDate = DateTime.Now;

            var result = _blogService.Update(b.Data);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}

/*
GET	    /admin/getUser	                    Admin	UserDetailDto	        ?userId=(int)
GET	    /admin/getBlog	                    Admin	BlogDetailDto	        ?blogId=(int)
POST    /admin/getBlogView                  Admin	HtmlBlog    	        BlogDetailDto /////////
GET 	/admin/getAllUserSummary            Admin	List<UserSummaryDto>	
GET 	/admin/getAllUser                   Admin	List<User>	
GET	    /admin/getAllBlog	                Admin	List<BlogSummaryDto>	
POST	/admin/addAuthor	                Admin		                    AuthorForRegister
GET	    /admin/getAllBlogByStatus	        Admin	List<Blog>	            ?status=(int)
POST	/admin/setOperationClaimToUser	    Admin	OK	                    OperationClaimToUserDto
POST	/admin/deleteOperationClaimToUser	Admin	OK	                    OperationClaimToUserDto
*/
