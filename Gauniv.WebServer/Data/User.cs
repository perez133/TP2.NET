using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gauniv.WebServer.Data
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        [StringLength(100)]
        public string Prenom { get; set; }

        // Already defined: games owned by the user.
        public List<Game> JeuxAchetes { get; set; } = new List<Game>();

        // Friend relationships (many-to-many self reference)
        public ICollection<UserFriend> Friends { get; set; } = new List<UserFriend>();
        public ICollection<UserFriend> FriendOf { get; set; } = new List<UserFriend>();
    }
}
