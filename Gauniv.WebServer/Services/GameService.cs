using Gauniv.WebServer.Data;
using Gauniv.WebServer.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GameDto>> GetAllGamesAsync()
        {
            return await _context.Games
                .Include(g => g.Categories) // Charge les catégories
                .Select(g => new GameDto
                {
                    Id = g.Id,
                    Nom = g.Nom,
                    Description = g.Description,
                    Prix = g.Prix,
                    Categories = g.Categories.Select(c => c.Nom).ToList()
                })
                .ToListAsync();
        }

        public async Task<GameDto> GetGameByIdAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Categories) // Charge les catégories
                .Where(g => g.Id == id)
                .Select(g => new GameDto
                {
                    Id = g.Id,
                    Nom = g.Nom,
                    Description = g.Description,
                    Prix = g.Prix,
                    Categories = g.Categories.Select(c => c.Nom).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}

