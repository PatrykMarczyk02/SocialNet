using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNet.Data;
using SocialNet.Models;
using System.Linq;

namespace SocialNet.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult UserManagement()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        
        [HttpPost]
        public IActionResult ChangeRole(int id, string role)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            user.Role = role;
            _context.SaveChanges();

            return RedirectToAction("UserManagement");
        }

        
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            if (user.Login == User.Identity.Name) 
            {
                ModelState.AddModelError("", "You cannot delete yourself.");
                return RedirectToAction("UserManagement");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("UserManagement");
        }
    }
}
