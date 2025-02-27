// File: Gauniv.WebServer/Api/AdminGamesController.cs
using Gauniv.WebServer.Dtos;
using Gauniv.WebServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gauniv.WebServer.Api
{
    [ApiController]
    [Route("api/admin/games")]
    [Authorize(Roles = "Admin")]
    public class AdminGamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public AdminGamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // POST: /api/admin/games
        [HttpPost]
        public async Task<ActionResult<GameDto>> AddGame([FromBody] AddGameRequest request)
        {
            var addedGame = await _gameService.AddGameAsync(request);
            return CreatedAtAction(nameof(GetGameById), new { id = addedGame.Id }, addedGame);
        }

        // PUT: /api/admin/games/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<GameDto>> UpdateGame(int id, [FromBody] UpdateGameRequest request)
        {
            var updatedGame = await _gameService.UpdateGameAsync(id, request);
            if (updatedGame == null)
                return NotFound();
            return Ok(updatedGame);
        }

        // DELETE: /api/admin/games/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _gameService.DeleteGameAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // GET: /api/admin/games/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGameById(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound();
            return Ok(game);
        }
    }
}
