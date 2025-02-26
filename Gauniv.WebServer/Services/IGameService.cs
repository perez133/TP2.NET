using Gauniv.WebServer.Dtos;

namespace Gauniv.WebServer.Services
{
    public interface IGameService
    {
        Task<List<GameDto>> GetAllGamesAsync();
        Task<GameDto> GetGameByIdAsync(int id);
    }
}
