// File: Controllers/HomeController.cs
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Load all games from the database.
            var dbGames = await _context.Games.ToListAsync();

            // Map database games to GameViewModel.
            var gameViewModels = dbGames.Select(g => new GameViewModel
            {
                Id = g.Id,
                Name = g.Nom,
                Description = g.Description,
                Price = g.Prix,
                // If you have an ImageUrl in your DB, map it; otherwise use a placeholder.
                ImageUrl = "/images/game-placeholder.jpg"
            }).ToList();

            // Load all categories.
            var dbCategories = await _context.Categories.ToListAsync();
            var categoryViewModels = dbCategories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Nom
            }).ToList();

            var model = new HomeViewModel
            {
                AllGames = gameViewModels,
                Categories = categoryViewModels
            };

            return View(model);
        }
    }
}
