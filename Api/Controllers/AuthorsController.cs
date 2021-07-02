using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomModel]
    public class AuthorsController : ControllerBase
    {
        private IUserService _userService;
        private IAuthorService _authorService;
        private IBlogService _blogService;
        private IBlogTagService _blogTagService;
        private ITagService _tagService;

        public AuthorsController(IAuthorService authorService, IBlogService blogService, IUserService userService, IBlogTagService blogTagService, ITagService tagService)
        {
            _authorService = authorService;
            _blogService = blogService;
            _userService = userService;
            _blogTagService = blogTagService;
            _tagService = tagService;
        }

        [HttpGet("getAuthor")]
        [Authorize]
        public IActionResult GetAuthor(int authorId)
        {
            var author = _authorService.GetById(authorId);
            if (!author.Success)
            {
                BadRequest(author);
            }
            return Ok(author.Data);
        }

        [HttpGet("getAuthorByName")]
        [Authorize]
        public IActionResult GetAuthor(string authorName)
        {
            var author = _authorService.GetByName(authorName);
            if (!author.Success)
            {
                BadRequest(author);
            }
            return Ok(author.Data);
        }

        //Tüm kullanıcıları cekme
        [HttpGet("getAllAuthor")]
        [Authorize]
        public IActionResult GetAllAuthor()
        {
            var author = _authorService.GetList().Data;
            return Ok(author);
        }

        //Yazar ekleme
        [HttpPost("getAuthorAccount")]
        [Authorize]
        public IActionResult GetAuthorAccount(AuthorForRegister authorForRegister)
        {
            var user = _userService.GetById(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)),Status.Per.System);
            if (user.Data == null)
            {
                return BadRequest();
            }
            if (!user.Success)
            {
                return BadRequest(user);
            }//gereksiz buralar

            if (((Status.Neno ^ user.Data.Status) & Status.Sban[3]) != 0)//Profil banı var mı?
            {
                return BadRequest(Messages.UserAuthorBan);
            }

            var authorExists = _authorService.AuthorExists(authorForRegister, user.Data.Id);
            if (authorExists.Success)//yazar mecut sa
            {
                return BadRequest(authorExists);
            }

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
                return BadRequest(author);
                //return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = author.Message });
            }
            return Ok(author);
        }

        //Yazar ekleme
        [HttpPost("postBlog")]
        [Authorize(Roles = "Author")]
        public IActionResult PostBlog(BlogDetailDto blogDetailDto)
        {
            var checkAuthor = _authorService.GetByUserId(Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)));//yazar
            if (checkAuthor.Data == null)
            {
                return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = Messages.AuthorNotFound });
            }

            var result = _blogService.Add(new Blog
            {
                AuthorId = 1,//checkAuthor.Data.Id,
                BlogTitle = blogDetailDto.BlogTitle,
                BlogSummary = blogDetailDto.BlogSummary,
                BlogTitlePhotoUrl = blogDetailDto.BlogTitlePhotoUrl,
                BlogDate = DateTime.Now,//blogDetailDto.BlogDate,
                BlogContent = JsonConvert.SerializeObject(blogDetailDto.BlogContent),//blogDetailDto.BlogContent,
                BlogStatus = 1
            });

            if (!result.Success)
            {
                return BadRequest(result);
                //return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message });
            }

            //         //////////////////////////////////////////////////////////// yeni tag konturolü
            //List<int> tagsId = blogDetailDto.BlogTags.Split(",").ToList();  //garip Linq kodu olacak

            if (result.Success)
            {
                List<Tag> tagNames = blogDetailDto.BlogTags;
                for (int i = 0; i < tagNames.Count; i++)
                {
                    _blogTagService.Add(new BlogTag
                    {
                        BlogId = result.Data,
                        TagId = _tagService.GetByName(tagNames[i].Name).Data.Id,
                    });
                }
            }

            return Ok(result);
        }
    }
}

/*
GET	    /authors/getAllAuthor	            Auth	Author	                
GET	    /authors/getAuthor	                Auth	List<Author>	        ?authorId=(int)
GET	    /authors/getAuthorByName            Auth	List<Author>	        ?authorName=(string)
POST	/authors/getAuthorAccount	        Auth	Ok	                    AuthorForRegister
POST	/authors/postBlog               	Author	OK	                    BlogDetailDto
*/
