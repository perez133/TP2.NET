// File: Data/Category.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Gauniv.WebServer.Data
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        // Add the navigation property:
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
