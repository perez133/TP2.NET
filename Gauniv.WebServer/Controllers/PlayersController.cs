// File: Controllers/PlayersController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public PlayersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Players
        public async Task<IActionResult> Index()
        {
            var players = await _context.Users.ToListAsync();
            return View(players);
        }
    }
}
