using Gauniv.WebServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gauniv.WebServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Populate sample featured games.
            var featuredGames = new List<GameViewModel>
            {
                new GameViewModel { Name = "Cyberpunk 2077", Description = "Futuristic RPG in an open world.", Price = 59.99m, ImageUrl = "/images/cyberpunk.jpg" },
                new GameViewModel { Name = "The Witcher 3", Description = "Epic RPG with rich story.", Price = 39.99m, ImageUrl = "/images/witcher3.jpg" },
                new GameViewModel { Name = "Age of Empires IV", Description = "Real-time strategy game.", Price = 49.99m, ImageUrl = "/images/aoe4.jpg" },
                // Add more as needed...
            };

            // Populate sample categories.
            var categories = new List<CategoryViewModel>
            {
                new CategoryViewModel { Id = 1, Name = "Action" },
                new CategoryViewModel { Id = 2, Name = "RPG" },
                new CategoryViewModel { Id = 3, Name = "Strategy" },
                // Add more if needed...
            };

            var model = new HomeViewModel
            {
                FeaturedGames = featuredGames,
                Categories = categories
            };

            return View(model);
        }
    }
}
