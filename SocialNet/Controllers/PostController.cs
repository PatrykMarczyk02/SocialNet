using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNet.Data;
using SocialNet.Models;
using System.Linq;

namespace SocialNet.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError("", "Content cannot be empty.");
                return View();
            }

            var post = new Post
            {
                Content = content,
                Author = User.Identity.Name, 
                CreatedAt = DateTime.UtcNow 
            };

            _context.Posts.Add(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public IActionResult List()
        {
            var posts = _context.Posts.OrderByDescending(p => p.CreatedAt).ToList();
            return View(posts);
        }

        [Authorize]
        public IActionResult MyPosts()
        {
            var userPosts = _context.Posts
                .Where(p => p.Author == User.Identity.Name)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(userPosts);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id && p.Author == User.Identity.Name);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, string content)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id && p.Author == User.Identity.Name);

            if (post == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError("", "Content cannot be empty.");
                return View(post);
            }

            post.Content = content;
            _context.SaveChanges();

            return RedirectToAction("MyPosts");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id && p.Author == User.Identity.Name);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("MyPosts");
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
