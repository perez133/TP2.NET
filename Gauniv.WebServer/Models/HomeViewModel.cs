using System.Collections.Generic;

namespace Gauniv.WebServer.Models
{
    public class HomeViewModel
    {
        public List<GameViewModel> FeaturedGames { get; set; } = new List<GameViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }

    public class GameViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        // You might add an ImageUrl property here to reference the cover art.
        public string ImageUrl { get; set; }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
