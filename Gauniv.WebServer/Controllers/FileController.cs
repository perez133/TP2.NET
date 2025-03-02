// File: Controllers/FileController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class FileController : Controller
    {
        // GET: /File/Download/{id}
        public async Task<IActionResult> Download(int id)
        {
            // Get the game from the database.
            // In a production scenario, you might not store the full binary in the DB.
            // Instead, you might store a reference (file path) to external storage.
            // For demonstration, we assume Payload contains the game binary.
            var game = await new Gauniv.WebServer.Data.ApplicationDbContext(null).Games.FindAsync(id);
            if (game == null)
                return NotFound();

            // Use a stream to avoid loading the entire file into memory.
            // In production, open a FileStream to an external file storage location.
            var stream = new MemoryStream(game.Payload);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = $"{game.Nom}.bin"
            };
        }
    }
}
