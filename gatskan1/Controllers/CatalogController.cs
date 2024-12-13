using Microsoft.AspNetCore.Mvc;

namespace gatskan1.Controllers
{
    public class Catalog : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Fantastics()
        {
            return View();
        }
        public IActionResult Fentesy()
        {
            return View();
        }
        public IActionResult Classic()
        {
            return View();
        }
        public IActionResult Detectiv()
        {
            return View();
        }
        public IActionResult Horror()
        {
            return View();
        }
    }
}
