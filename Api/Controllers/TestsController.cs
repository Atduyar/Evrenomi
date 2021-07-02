using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Filters;
using Core.Entities.Concrete;
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
    public class TestsController : ControllerBase
    {
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return Ok("Sen Admin Sin");
        }

        [HttpGet("Auth")]
        [Authorize]
        public IActionResult Auth()
        {
            return Ok("Sen Giris Yapmıs Sın");
        }

        [HttpPost("PostUser")]
        [Authorize]
        public IActionResult PostUser(UserForRegisterDto userForRegisterDto)
        {
            return Ok($"NickName = {userForRegisterDto.Nickname}   Email = {userForRegisterDto.Email}   :D");
        }

        [HttpGet("GetUser")]
        [Authorize]
        public IActionResult GetUser()
        {
            return Ok(new UserForRegisterDto()
            {
                Nickname = "TestIsmi",
                Email = "info@atduyar.com"
            });
        }

        [HttpGet("UserId")]
        [Authorize]
        public IActionResult UserId()
        {
            return Ok($"Sen in id'in = {User.FindFirstValue(ClaimTypes.NameIdentifier)} :D");
        }

        [HttpPost("PostComment")]
        public IActionResult PostComment(Comment comment)
        {
            return Ok(new Comment
            {
                text = $"Your comment is = {comment.text}   :D"
            });
        }

        [HttpGet("GetComment")]
        public IActionResult GetComment()
        {
            return Ok(new Comment
            {
                text = "Selam Dünya!"
            });
            //return Ok("Selam Dünya");
        }

        [HttpGet("OkComment")]
        public IActionResult OkComment()
        {
            return Ok(new Comment
            {
                text = "Ok aldın"
            });
        }

        [HttpGet("badRequestComment")]
        public IActionResult BadRequestComment()
        {
            return BadRequest();
        }

        [HttpGet("zafereSelamVer")]
        public IActionResult ZafereSelamVer()
        {
            return Ok(ConT.sendN());
        }
    }

    public class Comment
    {
        public string text { get; set; }
    }



    static class ConT
    {
        static public string sendN()
        {
            using (var client = new WebClient())
            {
                var request_json = "{\"app_id\":\"0011b0ec-2fce-4012-989a-56306b697f46\",\"include_player_ids\":[\"942c2993-b436-4b1e-be5b-c141c0777b2a\"],\"channel_for_external_user_ids\":\"push\",\"data\":{\"type\":\"blogCommentResponse\",\"blogId\":1,\"commentId\":75,\"commentResponse\":76},\"headings\":{\"en\":\"Birisi\"},\"contents\":{\"en\":\"Selam aldın.\"},\"small_icon\":\"ic_stat_onesignal_default\",\"android_group\":\"6\",\"template_id\":\"a4f3c3fe-8127-4eeb-a655-f5019bc7c10b\"}";
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers.Add("Authorization", "Basic " + "OGU5OGNkZDQtZjFlOS00MDM4LWE2NWEtNWI3ZWE0NGFjYmQz");
                var result = client.UploadString("https://onesignal.com/api/v1/notifications", "POST", request_json);

                Console.WriteLine(result);
                return ":D";
            }
        }
    }


}
