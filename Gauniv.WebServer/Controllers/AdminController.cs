// File: Gauniv.WebServer/Controllers/AdminController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games.Include(g => g.Categories).ToListAsync();
            return View(games);
        }

        // GET: /Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateGameViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Save the uploaded file to wwwroot/uploads folder
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, model.File.FileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await model.File.CopyToAsync(stream);
            }

            // For demonstration, read the file into binary and store it in the database.
            // In a production scenario, consider storing only the file path.
            var game = new Game
            {
                Nom = model.Name,
                Description = model.Description,
                Prix = model.Price,
                Payload = System.IO.File.ReadAllBytes(filePath)
            };

            if (model.CategoryIds != null && model.CategoryIds.Any())
            {
                var categories = await _context.Categories.Where(c => model.CategoryIds.Contains(c.Id)).ToListAsync();
                game.Categories = categories;
            }

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }

    public class CreateGameViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile File { get; set; }
        public int[] CategoryIds { get; set; }
    }
}
