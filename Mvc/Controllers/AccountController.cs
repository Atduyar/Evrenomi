using Microsoft.AspNetCore.Mvc;

namespace Mvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult ChangeProfile()
        {
            return View();
        }
    }
}
