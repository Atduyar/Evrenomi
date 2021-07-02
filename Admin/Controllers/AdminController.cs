using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Concrete;
using Admin.Models;
using Admin.Models.Results;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class AdminController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return RedirectToAction("Index","Login");
        }

        [Route("Index")]
        [Route("Admin")]
        public IActionResult Index(string id)
        {
            string t = HttpContext.Session.GetString("Token");
            ConT.t = t;
            var auth = ConnectionManager.Con.get("tests/auth");//girisi doğrular
            if (t == null || t == "" || !auth.Success)
            {
                return RedirectToAction("Index", "Login");
            }


            return View();
        }


        [Route("Admin/BlogsW")]
        public IActionResult BlogsW()
        {

            var blogs = ConnectionManager.Con.get<List<Blog>>("admin/getAllBlog", "", "GET",true);

            return View(new BlogsWModel
            {
                Token = HttpContext.Session.GetString("Token"),
                PendingBlogs = blogs.Data
            });
        }


        [Route("Admin/BlogsWC")]
        public IActionResult BlogsWC()
        {
            if (ConT.e < DateTime.Now)
            {
                Console.WriteLine("1");
            }
            var blogs = ConnectionManager.Con.get<List<Blog>>("admin/getAllBlog", "", "GET", true);

            return View(new BlogsWModel
            {
                Token = HttpContext.Session.GetString("Token"),
                PendingBlogs = blogs.Data
            });
        }

        
        [Route("Admin/UsersW")]
        public IActionResult UsersW()
        {

            var users = ConnectionManager.Con.get<List<User>>("admin/getAllUser", "", "GET", true);

            return View(new UsersWModel
            {
                Token = HttpContext.Session.GetString("Token"),
                Users = users.Data
            }); /*new BlogsWModel
            {
                Token = HttpContext.Session.GetString("Token"),
                PendingBlogs = blogs.Data
            });*/
        }

        [Route("Admin/BlogCreater")]
        public IActionResult BlogCreater()
        {
            return View();
        }
    }
}
