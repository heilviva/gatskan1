using Microsoft.AspNetCore.Mvc;

namespace gatskan1.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
