using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Gauniv.WebServer.Websocket;

namespace Gauniv.WebServer.Api
{
    [ApiController]
    [Route("api/players")]
    public class PlayersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public PlayersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /api/players
        [HttpGet]
        public async Task<IActionResult> GetPlayers()
        {
            var players = await _context.Users
                        .Select(u => new
                        {
                            u.Id,
                            u.UserName,
                            u.Nom,
                            u.Prenom,
                            Online = OnlineHub.ConnectedUsers.ContainsKey(u.Id)
                        })
                        .ToListAsync();
            return Ok(players);
        }
    }
}
