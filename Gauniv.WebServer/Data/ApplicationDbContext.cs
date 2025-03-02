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
        public DbSet<UserFriend> UserFriends { get; set; }  // For friend relationships

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ⚠️ Ne pas oublier cette ligne !

            // Configure IdentityUserLogin primary key fix
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(i => new { i.LoginProvider, i.ProviderKey });

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Nom = "Action" },
                new Category { Id = 2, Nom = "RPG" },
                new Category { Id = 3, Nom = "Stratégie" },
                new Category { Id = 4, Nom = "FPS" },
                new Category { Id = 5, Nom = "Puzzle" }
            );

            // Seed Games – now with six entries.
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = 1,
                    Nom = "Cyberpunk 2077",
                    Description = "Un RPG futuriste en monde ouvert.",
                    Payload = new byte[] { }, // Replace with actual file bytes if needed.
                    Prix = 59.99m
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
                },
                new Game
                {
                    Id = 4,
                    Nom = "DOOM Eternal",
                    Description = "An intense FPS with brutal combat.",
                    Payload = new byte[] { },
                    Prix = 59.99m
                },
                new Game
                {
                    Id = 5,
                    Nom = "Half-Life: Alyx",
                    Description = "A VR masterpiece that redefines the FPS genre.",
                    Payload = new byte[] { },
                    Prix = 59.99m
                },
                new Game
                {
                    Id = 6,
                    Nom = "Portal 2",
                    Description = "A mind-bending puzzle game with hilarious dialogue.",
                    Payload = new byte[] { },
                    Prix = 19.99m
                }
            );

            // Configure many-to-many relationship between Game and Category.
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Categories)
                .WithMany(c => c.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "GameCategory", // Name of the join table
                    j => j.HasOne<Category>()
                          .WithMany()
                          .HasForeignKey("CategoryId")
                          .HasConstraintName("FK_GameCategory_CategoryId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Game>()
                          .WithMany()
                          .HasForeignKey("GameId")
                          .HasConstraintName("FK_GameCategory_GameId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("GameId", "CategoryId");
                        j.HasData(
                            new { GameId = 1, CategoryId = 2 }, // Cyberpunk 2077 → RPG
                            new { GameId = 2, CategoryId = 2 }, // The Witcher 3 → RPG
                            new { GameId = 3, CategoryId = 3 }, // Age of Empires IV → Stratégie
                            new { GameId = 4, CategoryId = 1 }, // DOOM Eternal → Action
                            new { GameId = 4, CategoryId = 4 }, // DOOM Eternal → FPS
                            new { GameId = 5, CategoryId = 1 }, // Half-Life: Alyx → Action
                            new { GameId = 5, CategoryId = 4 }, // Half-Life: Alyx → FPS
                            new { GameId = 6, CategoryId = 5 }  // Portal 2 → Puzzle
                        );
                    }
                );

            // Configure friend relationships.
            modelBuilder.Entity<UserFriend>()
                .HasKey(uf => new { uf.UserId, uf.FriendId });

            modelBuilder.Entity<UserFriend>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.Friends)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFriend>()
                .HasOne(uf => uf.Friend)
                .WithMany(u => u.FriendOf)
                .HasForeignKey(uf => uf.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
