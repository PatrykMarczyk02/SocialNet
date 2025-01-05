using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SocialNet.Data;
using SocialNet.Models;
using System.Linq;
using System.Security.Claims;

namespace SocialNet.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Register()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Register(string email, string login, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match!");
                return View();
            }

            if (_context.Users.Any(u => u.Login == login || u.Email == email))
            {
                ModelState.AddModelError("", "User with the same email or login already exists!");
                return View();
            }

            var user = new User
            {
                Email = email,
                Login = login,
                Password = password, 
                Role = "user" 
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        
        public IActionResult Login()
        {
            return View("Login");
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login or password.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role) 
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

            
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

            TempData["Message"] = $"Welcome, {user.Login}!";
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

    }
}
