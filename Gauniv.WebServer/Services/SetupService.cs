using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Services
{
    public class SetupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public SetupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // Apply any pending migrations.
                if (context.Database.GetPendingMigrations().Any())
                {
                    await context.Database.MigrateAsync(cancellationToken);
                }

                // Seed Categories if none exist.
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Id = 1, Nom = "Action" },
                        new Category { Id = 2, Nom = "RPG" },
                        new Category { Id = 3, Nom = "Stratégie" },
                        new Category { Id = 4, Nom = "FPS" },
                        new Category { Id = 5, Nom = "Puzzle" }
                    );
                    await context.SaveChangesAsync();
                }

                var categories = context.Categories.ToList();

                // Seed Games if none exist.
                if (!context.Games.Any())
                {
                    context.Games.AddRange(
                        new Game
                        {
                            Id = 1,
                            Nom = "Cyberpunk 2077",
                            Description = "Un RPG futuriste en monde ouvert.",
                            Payload = new byte[] { }, // Replace with actual file bytes if needed.
                            Prix = 59.99m,
                            // Example: Cyberpunk 2077 is linked to Action (Id:1) and RPG (Id:2)
                            Categories = new System.Collections.Generic.List<Category>
                            {
                                categories.FirstOrDefault(c => c.Id == 1),
                                categories.FirstOrDefault(c => c.Id == 2)
                            }
                        },
                        new Game
                        {
                            Id = 2,
                            Nom = "The Witcher 3",
                            Description = "Un RPG épique avec une grande histoire.",
                            Payload = new byte[] { },
                            Prix = 39.99m,
                            // Linked to Aventure (using our seed: here we use Puzzle for example) and RPG.
                            Categories = new System.Collections.Generic.List<Category>
                            {
                                categories.FirstOrDefault(c => c.Id == 5),
                                categories.FirstOrDefault(c => c.Id == 2)
                            }
                        },
                        new Game
                        {
                            Id = 3,
                            Nom = "Age of Empires IV",
                            Description = "Un jeu de stratégie en temps réel.",
                            Payload = new byte[] { },
                            Prix = 49.99m,
                            // Linked to Stratégie (Id:3) and FPS (Id:4) for demonstration.
                            Categories = new System.Collections.Generic.List<Category>
                            {
                                categories.FirstOrDefault(c => c.Id == 3),
                                categories.FirstOrDefault(c => c.Id == 4)
                            }
                        },
                        new Game
                        {
                            Id = 4,
                            Nom = "DOOM Eternal",
                            Description = "An intense FPS with brutal combat.",
                            Payload = new byte[] { },
                            Prix = 59.99m,
                            // Linked to Action and FPS.
                            Categories = new System.Collections.Generic.List<Category>
                            {
                                categories.FirstOrDefault(c => c.Id == 1),
                                categories.FirstOrDefault(c => c.Id == 4)
                            }
                        },
                        new Game
                        {
                            Id = 5,
                            Nom = "Half-Life: Alyx",
                            Description = "A VR masterpiece that redefines the FPS genre.",
                            Payload = new byte[] { },
                            Prix = 59.99m,
                            // Linked to Action and FPS.
                            Categories = new System.Collections.Generic.List<Category>
                            {
                                categories.FirstOrDefault(c => c.Id == 1),
                                categories.FirstOrDefault(c => c.Id == 4)
                            }
                        },
                        new Game
                        {
                            Id = 6,
                            Nom = "Portal 2",
                            Description = "A mind-bending puzzle game with hilarious dialogue.",
                            Payload = new byte[] { },
                            Prix = 19.99m,
                            // Linked to Puzzle.
                            Categories = new System.Collections.Generic.List<Category>
                            {
                                categories.FirstOrDefault(c => c.Id == 5)
                            }
                        }
                    );
                    await context.SaveChangesAsync();
                }

                // Seed fake users and roles.
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Ensure roles exist.
                if (!await roleManager.RoleExistsAsync("Player"))
                    await roleManager.CreateAsync(new IdentityRole("Player"));
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                // Seed 10 fake players.
                for (int i = 1; i <= 10; i++)
                {
                    var userName = $"player{i}";
                    var email = $"player{i}@example.com";
                    var user = await userManager.FindByNameAsync(userName);
                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = userName,
                            Email = email,
                            Nom = $"Player{i}Name",
                            Prenom = $"Player{i}Surname"
                        };
                        await userManager.CreateAsync(user, "Password1!");
                        await userManager.AddToRoleAsync(user, "Player");
                    }
                }

                // Seed 1 fake admin.
                var adminUser = await userManager.FindByNameAsync("admin");
                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        Nom = "Admin",
                        Prenom = "User"
                    };
                    await userManager.CreateAsync(adminUser, "AdminPassword1!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
