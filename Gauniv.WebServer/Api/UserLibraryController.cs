using Gauniv.WebServer.Dtos;
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Api
{
    [ApiController]
    [Route("api/user/library")]
    [Authorize]
    public class UserLibraryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserLibraryController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /api/user/library
        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetOwnedGames()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users
                        .Include(u => u.JeuxAchetes)
                            .ThenInclude(g => g.Categories)
                        .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Unauthorized();

            var games = user.JeuxAchetes.Select(g => new GameDto
            {
                Id = g.Id,
                Nom = g.Nom,
                Description = g.Description,
                Prix = g.Prix,
                Categories = g.Categories.Select(c => c.Nom).ToList()
            }).ToList();

            return Ok(games);
        }

        // POST: /api/user/library/purchase
        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseGame([FromBody] PurchaseGameRequest request)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _context.Users
                        .Include(u => u.JeuxAchetes)
                        .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Unauthorized();

            var game = await _context.Games.Include(g => g.Categories).FirstOrDefaultAsync(g => g.Id == request.GameId);
            if (game == null)
                return NotFound("Game not found");

            if (user.JeuxAchetes.Any(g => g.Id == game.Id))
                return BadRequest("Game already purchased");

            user.JeuxAchetes.Add(game);
            await _context.SaveChangesAsync();
            return Ok("Game purchased successfully");
        }
    }

    public class PurchaseGameRequest
    {
        public int GameId { get; set; }
    }
}
