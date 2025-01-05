using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNet.Data; 
using SocialNet.Models; 
namespace SocialNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; 
        private readonly ILogger<HomeController> _logger;

        
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index()
        {
            
            var posts = _context.Posts.OrderByDescending(p => p.CreatedAt).ToList();

            if (User.Identity.IsAuthenticated)
            {
                
                if (User.IsInRole("admin"))
                {
                    
                    return View("AdminIndex", posts);
                }
                else if (User.IsInRole("user"))
                {
                    
                    return View("UserIndex", posts);
                }
            }
            return View("GuestIndex", posts);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
