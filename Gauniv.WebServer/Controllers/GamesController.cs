using Gauniv.WebServer.Dtos;
using Gauniv.WebServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gauniv.WebServer.Controllers
{
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }
        
        [Route("api/games")]
        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetAllGames()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Ok(games);
        }

        [Route("api/games/{id}")]
        [HttpGet]
        public async Task<ActionResult<GameDto>> GetGameById(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }
    }
}
