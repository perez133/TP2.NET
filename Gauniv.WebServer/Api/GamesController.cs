// File: Gauniv.WebServer/Api/GamesController.cs
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Dtos;
using Gauniv.WebServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Gauniv.WebServer.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly UserManager<User> _userManager;

        public GamesController(IGameService gameService, UserManager<User> userManager)
        {
            _gameService = gameService;
            _userManager = userManager;
        }

        // GET: /api/games?owned=true&offset=10&limit=15&category[]=3&category[]=2
        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetGames(
            [FromQuery] bool owned = false,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 20,
            [FromQuery(Name = "category[]")] int[] categories = null)
        {
            string userId = null;
            if (owned)
            {
                if (!User.Identity.IsAuthenticated)
                    return Unauthorized("Authentication required for owned games");

                userId = _userManager.GetUserId(User);
            }

            var queryParams = new GameQueryParams
            {
                Owned = owned,
                Offset = offset,
                Limit = limit,
                CategoryIds = categories?.ToList() ?? new List<int>()
            };

            var games = await _gameService.GetGamesAsync(queryParams, userId);
            return Ok(games);
        }

        // GET: /api/games/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGameById(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound();

            return Ok(game);
        }

        // POST: /api/games
        [HttpPost]
        public async Task<ActionResult<GameDto>> AddGame([FromBody] AddGameRequest request)
        {
            var addedGame = await _gameService.AddGameAsync(request);
            return CreatedAtAction(nameof(GetGameById), new { id = addedGame.Id }, addedGame);
        }

        // PUT: /api/games/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<GameDto>> UpdateGame(int id, [FromBody] UpdateGameRequest request)
        {
            var updatedGame = await _gameService.UpdateGameAsync(id, request);
            if (updatedGame == null)
                return NotFound();

            return Ok(updatedGame);
        }

        // DELETE: /api/games/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _gameService.DeleteGameAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // GET: /api/games/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<GameDto>>> GetByCategory(int categoryId)
        {
            var games = await _gameService.GetGamesByCategoryAsync(categoryId);
            return Ok(games);
        }

        // GET: /api/games/price?min=20&max=45
        [HttpGet("price")]
        public async Task<ActionResult<List<GameDto>>> GetByPrice([FromQuery] decimal min, [FromQuery] decimal max)
        {
            var games = await _gameService.GetGamesByPriceAsync(min, max);
            return Ok(games);
        }

        // GET: /api/games/{id}/download
        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadGame(int id)
        {
            // Retrieve the game from the database to access its Payload.
            // (Casting _gameService to GameService to expose _context.)
            var game = await ((GameService)_gameService)._context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            if (game.Payload == null || game.Payload.Length == 0)
                return NotFound("Game binary not found");

            // Stream the payload from memory.
            var stream = new MemoryStream(game.Payload);
            return File(stream, "application/octet-stream", $"{game.Nom}.bin");
        }
    }

    // Request DTOs for adding/updating games.
    public class AddGameRequest
    {
        public string Nom { get; set; }
        public string Description { get; set; }
        // The payload is sent as a Base64 encoded string.
        public string PayloadBase64 { get; set; }
        public decimal Prix { get; set; }
        public List<int> CategoryIds { get; set; }
    }

    public class UpdateGameRequest
    {
        public string Nom { get; set; }
        public string Description { get; set; }
        // Optional new payload, Base64 encoded.
        public string PayloadBase64 { get; set; }
        public decimal? Prix { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
