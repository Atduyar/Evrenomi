using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Admin.Concrete;
using Admin.Models;
using Admin.Models.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserForLogin userForLogin)
        {
            var json = JsonConvert.SerializeObject(new
                {EmailOrNickname = userForLogin.EmailOrNickname, Password = userForLogin.Password});
            string url = "https://api.atduyar.com/api/auth/login";
            var x = ConT.getT(json,HttpContext);

            if (x == "")
            {
                return View();
            }
            else
            {
                //var dt = ConnectionManager.Con.get("tests/Auth");
                return RedirectToAction("Index", "Admin");
            }

        }

    }
}
