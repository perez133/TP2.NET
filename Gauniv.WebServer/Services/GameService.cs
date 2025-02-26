using Gauniv.WebServer.Data;
using Gauniv.WebServer.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Dapper;


namespace Gauniv.WebServer.Services
{
    public class GameService : IGameService
    {
        
        private ApplicationDbContext? applicationDbContext;
        private readonly IServiceProvider serviceProvider;
        public GameService(IServiceProvider serviceProvider)
        {
           this.serviceProvider = serviceProvider;
        }
        public List<GameDto> GetAllGames()
        {
            using (var scope = serviceProvider.CreateScope()) {
                applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                return applicationDbContext.Games
            .Include(g => g.Categories) // ✅ Charge les catégories associées
            .Select(g => new GameDto
            {
                Id = g.Id,
                Nom = g.Nom,
                Description = g.Description,
                Prix = g.Prix,
                Categories = g.Categories.Select(c => c.Nom).ToList() // ✅ Transforme en liste de noms de catégories
            })
            .ToList();
            }
            
        }

        public GameDto GetGameById(int id)
        {
            // using (var connection = new NpgsqlConnection(_connectionString))
            // {
            //     connection.Open(); // Ouvrir la connexion explicitement
            //
            //     string query = @"
            //         SELECT *
            //         FROM public.""Games""
            //         WHERE ""Id"" = @Id"; // IMPORTANT : public."Games"
            //
            //     return connection.QueryFirstOrDefault<GameDto>(query, new { Id = id });
            // }
            return null;
        }



    }
}

