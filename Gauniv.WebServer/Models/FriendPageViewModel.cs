using System.Collections.Generic;
using Gauniv.WebServer.Data;

namespace Gauniv.WebServer.Models
{
    public class FriendPageViewModel
    {
        // Accepted friends.
        public IEnumerable<User> Friends { get; set; } = new List<User>();

        // Pending friend requests (received by current user).
        public IEnumerable<UserFriend> Requests { get; set; } = new List<UserFriend>();

        // Search results (users matching the search query).
        public IEnumerable<User> SearchResults { get; set; } = new List<User>();

        // The current search query.
        public string SearchQuery { get; set; }
    }
}
