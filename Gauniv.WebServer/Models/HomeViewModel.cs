// File: Models/HomeViewModel.cs
namespace Gauniv.WebServer.Models
{
    public class HomeViewModel
    {
        public List<GameViewModel> AllGames { get; set; } = new List<GameViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }

    public class GameViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Maps to Game.Nom
        public string Description { get; set; }
        public decimal Price { get; set; }
        // For demonstration purposes, we use a placeholder image URL.
        public string ImageUrl { get; set; } = "/images/game-placeholder.jpg";
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Maps to Category.Nom
    }
}
