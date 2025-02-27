namespace Gauniv.WebServer.Data
{
    public class UserFriend
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string FriendId { get; set; }
        public User Friend { get; set; }
    }
}
