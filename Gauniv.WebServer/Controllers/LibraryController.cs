// File: Controllers/LibraryController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public LibraryController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Library
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Load owned games (JeuxAchetes) including categories.
            var userWithGames = await _context.Users
                .Include(u => u.JeuxAchetes)
                    .ThenInclude(g => g.Categories)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return View(userWithGames.JeuxAchetes);
        }
    }
}
