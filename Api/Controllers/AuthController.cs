using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Filters;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomModel]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin);
            }
            if ((((Status.Neno ^ userToLogin.Data.Status) & Status.DontOpen) != 0))
            {
                return BadRequest(new ErrorResult(message: Messages.UserNotFound + " | Banlı yada onaylanmamıs Hesap"));
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(new ErrorResponseDto {Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message});
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExistsEmail = _authService.UserExists(userForRegisterDto.Email);
            if (userExistsEmail.Success)
            {
                return BadRequest(userExistsEmail);
            }

            var userExistsNickname = _authService.UserExists(userForRegisterDto.Nickname);
            if (userExistsNickname.Success)
            {
                return BadRequest(userExistsNickname);
            }
            //kullanıcı adı veya eposta eposta veya kullanıcı adı olarak kayıtlı mı var mı??

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("nicknameControl")]
        public IActionResult NicknameControl(string data)
        {
            var userExistsNickname = _authService.UserExists(data);
            if (userExistsNickname.Success)
            {
                //return BadRequest(userExistsNickname);
                return BadRequest(new ErrorResult(message: userExistsNickname.Message));
            }

            return Ok(new SuccessResult(message: userExistsNickname.Message));
        }

        [HttpGet("emailControl")]
        public IActionResult EmailControl(string data)
        {
            var userExistsEmail = _authService.UserExists(data);
            if (userExistsEmail.Success)
            {
                //return BadRequest(userExistsEmail);
                return BadRequest(new ErrorResult(message: userExistsEmail.Message));
            }

            return Ok(new SuccessResult(message: userExistsEmail.Message));
        }
    }
}

/*
POST	/auth/login		Token	UserForLoginDto
POST	/auth/register		Token	UserForRegisterDto
*/
