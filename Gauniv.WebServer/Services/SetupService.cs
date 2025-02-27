using Gauniv.WebServer.Data;
using Gauniv.WebServer.Migrations;
using Gauniv.WebServer.Websocket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Emit;
using System.Text;

namespace Gauniv.WebServer.Services
{
    public class SetupService : IHostedService
    {
        private ApplicationDbContext? applicationDbContext;
        private readonly IServiceProvider serviceProvider;
        private Task? task;

        public SetupService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
            {
                applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                if(applicationDbContext is null)
                {
                    throw new Exception("ApplicationDbContext is null");
                }

                if (applicationDbContext.Database.GetPendingMigrations().Any())
                {
                    applicationDbContext.Database.Migrate();
                }


                // Ajouter ici les données que vous insérer dans votre DB au démarrage

                // Création auto de la table intermédiaire
                if (applicationDbContext.Categories.Count() == 0)
                {
                    applicationDbContext.Categories.AddRange(
                        new Category { Id = 1, Nom = "Action" },
                        new Category { Id = 2, Nom = "RPG" },
                        new Category { Id = 3, Nom = "Stratégie" },
                        new Category { Id = 4, Nom = "Multijoueur" },
                        new Category { Id = 5, Nom = "Aventure" });
                    applicationDbContext.SaveChanges();
                }

                var Categorie = applicationDbContext.Categories.ToList();

                if (applicationDbContext.Games.Count() == 0)
                {
                    applicationDbContext.Games.AddRange(new Game
                    {
                        Id = 1,
                        Nom = "Cyberpunk 2077",
                        Description = "Un RPG futuriste en monde ouvert.",
                        Payload = new byte[] { }, // Peut être remplacé par un fichier réel
                        Prix = 59.99m,
                        Categories = new List<Category>
                        {
                            Categorie.FirstOrDefault(c => c.Id == 1), // ✅ Aventure
                            Categorie.FirstOrDefault(c => c.Id == 2)  // ✅ RPG
                        }
                        //Categories = new List<Category> { new Category { Id = 2, Nom = "RPG" } }

                    },
                    new Game
                    {
                        Id = 2,
                        Nom = "The Witcher 3",
                        Description = "Un RPG épique avec une grande histoire.",
                        Payload = new byte[] { },
                        Prix = 39.99m,
                        Categories = new List<Category>
                        {
                            Categorie.FirstOrDefault(c => c.Id == 5), // ✅ Aventure
                            Categorie.FirstOrDefault(c => c.Id == 2)  // ✅ RPG
                        }
                    },
                    new Game
                    {
                        Id = 3,
                        Nom = "Age of Empires IV",
                        Description = "Un jeu de stratégie en temps réel.",
                        Payload = new byte[] { },
                        Prix = 49.99m,
                        Categories = new List<Category>
                        {
                            Categorie.FirstOrDefault(c => c.Id == 3), // ✅ Aventure
                            Categorie.FirstOrDefault(c => c.Id == 4)  // ✅ RPG
                        }
                    });
                    applicationDbContext.SaveChanges();
                }
                // Ajout de jeux d'exemple
               

            return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
