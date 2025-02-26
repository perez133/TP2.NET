using Gauniv.WebServer.Data;
using Gauniv.WebServer.Dtos;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Dapper;


namespace Gauniv.WebServer.Services
{
    public class GameService : IGameService
    {
        private readonly string _connectionString;

        public GameService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<GameDto> GetAllGames()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open(); // Ouvrir la connexion explicitement

                string query = @"
                    SELECT *
                    FROM public.""Games""";  // IMPORTANT : public."Games"

                return connection.Query<GameDto>(query).ToList();
            }
        }

        public GameDto GetGameById(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open(); // Ouvrir la connexion explicitement

                string query = @"
                    SELECT *
                    FROM public.""Games""
                    WHERE ""Id"" = @Id"; // IMPORTANT : public."Games"

                return connection.QueryFirstOrDefault<GameDto>(query, new { Id = id });
            }
        }



    }
}

