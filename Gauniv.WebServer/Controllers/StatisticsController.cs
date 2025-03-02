// File: Controllers/StatisticsController.cs
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Statistics
        public async Task<IActionResult> Index()
        {
            // Total number of games available.
            var totalGames = await _context.Games.CountAsync();

            // Number of games per category.
            var gamesPerCategory = await _context.Categories
                .Select(c => new { Category = c.Nom, Count = c.Games.Count })
                .ToListAsync();

            // Average number of games owned per account.
            var avgGamesPerAccount = await _context.Users.AverageAsync(u => u.JeuxAchetes.Count);

            // For demonstration, other statistics (average time played, max simultaneous players) are stubbed.
            var stats = new StatisticsViewModel
            {
                TotalGames = totalGames,
                GamesPerCategory = gamesPerCategory,
                AvgGamesPerAccount = avgGamesPerAccount,
                AvgTimePlayedPerGame = 0, // Stub: implement time tracking in your application.
                MaxSimultaneousPlayersOverall = 0, // Stub
                MaxSimultaneousPlayersPerGame = 0 // Stub
            };

            return View(stats);
        }
    }
}
