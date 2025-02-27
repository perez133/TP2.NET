using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Relation Many-to-Many
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
