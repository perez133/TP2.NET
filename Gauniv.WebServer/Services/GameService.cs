// File: Gauniv.WebServer/Services/GameService.cs
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gauniv.WebServer.Api;
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Services
{
    public class GameService : IGameService
    {
        // Exposed for use by the download endpoint.
        public readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GameService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GameDto>> GetGamesAsync(GameQueryParams query, string userId)
        {
            IQueryable<Game> gamesQuery;

            if (query.Owned && !string.IsNullOrEmpty(userId))
            {
                var user = await _context.Users
                    .Include(u => u.JeuxAchetes)
                        .ThenInclude(g => g.Categories)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return new List<GameDto>();

                gamesQuery = user.JeuxAchetes.AsQueryable();
            }
            else
            {
                gamesQuery = _context.Games.Include(g => g.Categories);
            }

            if (query.CategoryIds != null && query.CategoryIds.Any())
            {
                gamesQuery = gamesQuery.Where(g => g.Categories.Any(c => query.CategoryIds.Contains(c.Id)));
            }

            int offset = query.Offset ?? 0;
            int limit = query.Limit ?? 20;
            gamesQuery = gamesQuery.Skip(offset).Take(limit);

            return await gamesQuery
                .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<GameDto> GetGameByIdAsync(int id)
        {
            var game = await _context.Games.Include(g => g.Categories)
                                           .FirstOrDefaultAsync(g => g.Id == id);
            return game == null ? null : _mapper.Map<GameDto>(game);
        }

        public async Task<GameDto> AddGameAsync(AddGameRequest request)
        {
            var newGame = new Game
            {
                Nom = request.Nom,
                Description = request.Description,
                Prix = request.Prix,
                // Convert Base64 payload to byte array.
                Payload = string.IsNullOrEmpty(request.PayloadBase64) ? new byte[] { } : Convert.FromBase64String(request.PayloadBase64)
            };

            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                var categories = await _context.Categories.Where(c => request.CategoryIds.Contains(c.Id)).ToListAsync();
                newGame.Categories = categories;
            }

            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();
            return _mapper.Map<GameDto>(newGame);
        }

        public async Task<GameDto> UpdateGameAsync(int id, UpdateGameRequest request)
        {
            var game = await _context.Games.Include(g => g.Categories)
                                           .FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                return null;

            if (!string.IsNullOrEmpty(request.Nom))
                game.Nom = request.Nom;
            if (!string.IsNullOrEmpty(request.Description))
                game.Description = request.Description;
            if (request.Prix.HasValue)
                game.Prix = request.Prix.Value;
            if (!string.IsNullOrEmpty(request.PayloadBase64))
                game.Payload = Convert.FromBase64String(request.PayloadBase64);

            if (request.CategoryIds != null)
            {
                game.Categories.Clear();
                var categories = await _context.Categories.Where(c => request.CategoryIds.Contains(c.Id)).ToListAsync();
                game.Categories = categories;
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<GameDto>(game);
        }

        public async Task<bool> DeleteGameAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return false;

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GameDto>> GetGamesByCategoryAsync(int categoryId)
        {
            var games = await _context.Games
                .Include(g => g.Categories)
                .Where(g => g.Categories.Any(c => c.Id == categoryId))
                .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return games;
        }

        public async Task<List<GameDto>> GetGamesByPriceAsync(decimal min, decimal max)
        {
            var games = await _context.Games
                .Include(g => g.Categories)
                .Where(g => g.Prix >= min && g.Prix <= max)
                .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return games;
        }
    }

    public class GameQueryParams
    {
        public bool Owned { get; set; }
        public int? Offset { get; set; }
        public int? Limit { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
