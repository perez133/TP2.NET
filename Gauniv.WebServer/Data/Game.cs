using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Gauniv.WebServer.Data
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public byte[] Payload { get; set; } // Stockage binaire du jeu

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prix { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>(); // Un jeu peut avoir plusieurs catégories
    }
}
