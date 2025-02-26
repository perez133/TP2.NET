using Gauniv.WebServer.Dtos;

namespace Gauniv.WebServer.Services
{
    public interface IGameService
    {
        List<GameDto> GetAllGames();
        GameDto GetGameById(int id);
    }
}
