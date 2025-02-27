using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Api
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/statistics
        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var totalGames = await _context.Games.CountAsync();

            var gamesByCategory = await _context.Categories
                                  .Select(c => new
                                  {
                                      Category = c.Nom,
                                      Count = _context.Games.Count(g => g.Categories.Any(cat => cat.Id == c.Id))
                                  })
                                  .ToListAsync();

            var averageGamesPerUser = await _context.Users.AverageAsync(u => u.JeuxAchetes.Count);

            // For play time and max concurrent players, we return null as placeholders.
            var averagePlayTimePerGame = (double?)null;
            var maxConcurrentPlayers = (int?)null;

            return Ok(new
            {
                TotalGames = totalGames,
                GamesByCategory = gamesByCategory,
                AverageGamesPerUser = averageGamesPerUser,
                AveragePlayTimePerGame = averagePlayTimePerGame,
                MaxConcurrentPlayers = maxConcurrentPlayers
            });
        }
    }
}
