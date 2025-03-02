// File: Controllers/HomeController.cs
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Home/Index?search=...&minPrice=...&maxPrice=...&category=...&owned=true
        public async Task<IActionResult> Index(string search, decimal? minPrice, decimal? maxPrice, int? category, bool owned = false, int offset = 0, int limit = 15)
        {
            var query = _context.Games.Include(g => g.Categories).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(g => g.Nom.Contains(search));

            if (minPrice.HasValue)
                query = query.Where(g => g.Prix >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(g => g.Prix <= maxPrice.Value);

            if (category.HasValue)
                query = query.Where(g => g.Categories.Any(c => c.Id == category.Value));

            if (owned && User.Identity.IsAuthenticated)
            {
                // Filter games that are owned by the logged-in user.
                var currentUser = await _userManager.GetUserAsync(User);
                query = query.Where(g => currentUser.JeuxAchetes.Any(j => j.Id == g.Id));
            }

            var games = await query.Skip(offset).Take(limit).ToListAsync();
            var categories = await _context.Categories.ToListAsync();

            var model = new HomeViewModel
            {
                AllGames = games.Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Name = g.Nom,
                    Description = g.Description,
                    Price = g.Prix,
                    ImageUrl = "/images/game-placeholder.jpg" // Replace with actual image if available.
                }).ToList(),
                Categories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Nom
                }).ToList()
            };

            return View(model);
        }
    }
}
