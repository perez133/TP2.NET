using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ⚠️ Ne pas oublier cette ligne !

            // ✅ Correction de l’erreur de clé primaire pour IdentityUserLogin<string>
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(i => new { i.LoginProvider, i.ProviderKey });
            // Ajout de catégories d'exemple
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Nom = "Action" },
                new Category { Id = 2, Nom = "RPG" },
                new Category { Id = 3, Nom = "Stratégie" }
            );

            // Ajout de jeux d'exemple
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Nom = "Cyberpunk 2077",
                    Description = "Un RPG futuriste en monde ouvert.",
                    Payload = new byte[] { }, // Peut être remplacé par un fichier réel
                    Prix = 59.99m,
                    //Categories = new List<Category> { new Category { Id = 2, Nom = "RPG" } }

                },
                new Game
                {
                    Id = 2,
                    Nom = "The Witcher 3",
                    Description = "Un RPG épique avec une grande histoire.",
                    Payload = new byte[] { },
                    Prix = 39.99m
                },
                new Game
                {
                    Id = 3,
                    Nom = "Age of Empires IV",
                    Description = "Un jeu de stratégie en temps réel.",
                    Payload = new byte[] { },
                    Prix = 49.99m
                }
            );
        }
    }
}
