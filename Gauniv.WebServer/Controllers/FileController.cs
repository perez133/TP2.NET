using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Models;

namespace Gauniv.WebServer.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /File/Download/{id}
        public async Task<IActionResult> Download(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            if (game.Payload == null || game.Payload.Length == 0)
            {
                return NotFound("Game payload is empty.");
            }

            // Mark game as downloaded: store the game ID in session.
            var downloadedJson = HttpContext.Session.GetString("DownloadedGameIds");
            var downloadedIds = downloadedJson != null
                ? System.Text.Json.JsonSerializer.Deserialize<List<int>>(downloadedJson)
                : new List<int>();

            if (!downloadedIds.Contains(game.Id))
            {
                downloadedIds.Add(game.Id);
                HttpContext.Session.SetString("DownloadedGameIds", System.Text.Json.JsonSerializer.Serialize(downloadedIds));
            }

            var stream = new MemoryStream(game.Payload);
            return File(stream, "application/octet-stream", $"{game.Nom}.exe");
        }

        // GET: /File/Launch/{id}
        public async Task<IActionResult> Launch(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound("Game not found.");
            }
            // Check if the game is marked as downloaded in session.
            var downloadedJson = HttpContext.Session.GetString("DownloadedGameIds");
            var downloadedIds = downloadedJson != null
                ? System.Text.Json.JsonSerializer.Deserialize<List<int>>(downloadedJson)
                : new List<int>();

            if (!downloadedIds.Contains(game.Id))
            {
                return BadRequest("Game has not been downloaded yet.");
            }

            // Simulate launching the game. In a real scenario, you'd trigger a client-side action.
            var model = new LaunchGameViewModel
            {
                GameId = game.Id,
                GameName = game.Nom,
                Status = "Launched"
            };

            return View(model);
        }

        // POST: /File/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Remove the game from session (simulate deletion from library)
            var downloadedJson = HttpContext.Session.GetString("DownloadedGameIds");
            var downloadedIds = downloadedJson != null
                ? System.Text.Json.JsonSerializer.Deserialize<List<int>>(downloadedJson)
                : new List<int>();

            if (downloadedIds.Contains(id))
            {
                downloadedIds.Remove(id);
                HttpContext.Session.SetString("DownloadedGameIds", System.Text.Json.JsonSerializer.Serialize(downloadedIds));
            }

            TempData["Success"] = "Game deleted from library.";
            return RedirectToAction("Index", "Library");
        }
    }
}
