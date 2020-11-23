using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Filters;
using Business.Abstract;
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
                return BadRequest(new ErrorResponseDto{Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = userToLogin.Message,});
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new ErrorResponseDto {Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message});
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);
            if (userExists.Success)
            {
                return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = userExists.Message});
            }
            //kullanıcı adı veya eposta eposta veya kullanıcı adı olarak kayıtlı mı var mı??

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(new ErrorResponseDto { Operation = System.Reflection.MethodBase.GetCurrentMethod().Name, ErrorMessages = result.Message});
        }
    }
}
