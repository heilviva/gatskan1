using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using gatskan1.Models;

namespace gatskan1.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookShContext _context;
        public AccountController(BookShContext context) { _context = context; }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string firstname, string lastname, string email, string phone, string address, string password)
        {
            if (_context.Users.Any(u => u. Email == email))
            {
                ViewBag.ErrorMessage = "Пользователь с такими email уже существует";
                return View();
            }

            string hashedpassword = HashPassword(password);

            var user = new User
            {
                Username = firstname,
                Email = email,
                Phone = phone,
                Password = hashedpassword,
            };
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Account");


        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            string hashpassword = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == hashpassword);
            if (user != null)
            {
                var claims = new List<Claim>
           {
               new Claim(ClaimTypes.Name, user.Username),
               new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString())
                };

                var claimsIdent = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdent);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Неверные учетные данные";
            return View();
        }


    }
}

