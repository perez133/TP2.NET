// File: Gauniv.WebServer/Controllers/HomeController.cs
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games.Include(g => g.Categories).ToListAsync();
            return View(games);
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
