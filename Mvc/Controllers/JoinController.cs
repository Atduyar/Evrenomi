using Microsoft.AspNetCore.Mvc;

namespace Mvc.Controllers
{
    public class JoinController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
    }
}
