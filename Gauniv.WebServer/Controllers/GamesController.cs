// File: Controllers/GamesController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Games
        // Optionally accept query parameters for filtering: name, price range, category, owned (boolean)
        public async Task<IActionResult> Index(string search, decimal? minPrice, decimal? maxPrice, int? category, bool? owned, int offset = 0, int limit = 15)
        {
            // Base query: all games
            var query = _context.Games.Include(g => g.Categories).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(g => g.Nom.Contains(search));

            if (minPrice.HasValue)
                query = query.Where(g => g.Prix >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(g => g.Prix <= maxPrice.Value);

            if (category.HasValue)
                query = query.Where(g => g.Categories.Any(c => c.Id == category.Value));

            // owned filtering could be implemented here if the user is logged in.
            // For demonstration, we assume "owned" filtering would require checking the logged-in user's JeuxAchetes collection.

            var games = await query.Skip(offset).Take(limit).ToListAsync();
            return View(games);
        }
    }
}
