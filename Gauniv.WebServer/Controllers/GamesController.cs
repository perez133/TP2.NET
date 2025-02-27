using Gauniv.WebServer.Dtos;
using Gauniv.WebServer.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        [Route("api/games")]
        [HttpGet]
        public ActionResult<List<GameDto>> GetAllGames()
        {
            return _gameService.GetAllGames();
             
        }

        [Route("api/games/{id}")]
        [HttpGet]
        public ActionResult<GameDto> GetGameById(int id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            return game;
        }
    }
}
