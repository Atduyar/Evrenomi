using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IUserService _userService;
        private IOperationClaimService _operationClaimService;
        private IUserOperationClaimService _userOperationClaimService;

        public AdminController(IUserService userService, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService)
        {
            _userService = userService;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
        }

        //Rol Atama
        [HttpPost("setOperationClaimToUser")]
        [Authorize(Roles = "Admin")]
        public IActionResult SetOperationClaimToUser(OperationClaimToUserDto operationClaimToUserDto)
        {
            var user = _userService.GetByEmailOrNickname(operationClaimToUserDto.UserForLoginDto.EmailOrNickname);//user
            var claims = _userService.GerClaims(user);//userin rolleri
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
            var user = _userService.GetByEmailOrNickname(operationClaimToUserDto.UserForLoginDto.EmailOrNickname);//user
            var claims = _userService.GerClaims(user);//userin rolleri
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

        [HttpGet("testAdmin")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            return Ok("Sen Admin Sin");
        }

        [HttpGet("testAuth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok("Sen Giris Yapmıs Sın");
        }

        [HttpPost("testPostUser")]
        [Authorize]
        public IActionResult TestPostUser(UserForRegisterDto userForRegisterDto)
        {
            return Ok($"NickName = {userForRegisterDto.Nickname}   Email = {userForRegisterDto.Email}   :D");
        }

        [HttpGet("testGetUser")]
        [Authorize]
        public IActionResult TestPostUser()
        {
            return Ok(new User
            {
                Nickname = "TestIsmi",
                Email = "info@atduyar.com"
            });
        }

        [HttpPost("testPostComment")]
        public IActionResult TestPostComment(Comment comment)
        {
            return Ok(new Comment
            {
                text = $"Your comment is = {comment.text}   :D"
            });
        }

        [HttpGet("testGetComment")]
        public IActionResult TestGetComment()
        {
            return Ok(new Comment
            {
                text = "Selam Dünya!"
            });
            //return Ok("Selam Dünya");
        }

        [HttpGet("testUserId")]
        public IActionResult TestUserId()
        {
            return Ok($"Sen in id'in = {User.FindFirstValue(ClaimTypes.NameIdentifier)} :D");
        }

        [HttpGet("testOk")]
        public IActionResult TestOk()
        {
            return Ok(new Comment
            {
                text = "Ok aldın"
            });
        }

        [HttpGet("testBadRequest")]
        public IActionResult TestBadRequest()
        {
            return BadRequest(new Comment
            {
                text = "BadRequest aldın"
            });
        }
    }

    public class Comment
    {
        public string text { get; set; }
    }
}
