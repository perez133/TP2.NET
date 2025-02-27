using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Gauniv.WebServer.Models
{
    public class CreateViewModel()
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile File { get; set; }

        // IDs of selected categories
        public int[] CategoryIds { get; set; }
    }
    public class EditViewModel()
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile File { get; set; }

        public int[] CategoryIds { get; set; }
    }
}
