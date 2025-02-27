// File: Gauniv.WebServer/Services/IGameService.cs
using Gauniv.WebServer.Api;
using Gauniv.WebServer.Dtos;

namespace Gauniv.WebServer.Services
{
    public interface IGameService
    {
        Task<List<GameDto>> GetGamesAsync(GameQueryParams query, string userId);
        Task<GameDto> GetGameByIdAsync(int id);
        Task<GameDto> AddGameAsync(AddGameRequest request);
        Task<GameDto> UpdateGameAsync(int id, UpdateGameRequest request);
        Task<bool> DeleteGameAsync(int id);
        Task<List<GameDto>> GetGamesByCategoryAsync(int categoryId);
        Task<List<GameDto>> GetGamesByPriceAsync(decimal min, decimal max);
    }
}
