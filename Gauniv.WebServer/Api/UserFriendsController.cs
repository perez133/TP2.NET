using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Api
{
    [ApiController]
    [Route("api/user/friends")]
    [Authorize]
    public class UserFriendsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserFriendsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /api/user/friends
        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            var userId = _userManager.GetUserId(User);
            var friends = await _context.UserFriends
                            .Include(uf => uf.Friend)
                            .Where(uf => uf.UserId == userId)
                            .Select(uf => new { uf.Friend.Id, uf.Friend.UserName, uf.Friend.Nom, uf.Friend.Prenom })
                            .ToListAsync();
            return Ok(friends);
        }

        // POST: /api/user/friends/add
        [HttpPost("add")]
        public async Task<IActionResult> AddFriend([FromBody] AddFriendRequest request)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == request.FriendId)
                return BadRequest("You cannot add yourself as a friend");

            var exists = await _context.UserFriends.AnyAsync(uf => uf.UserId == userId && uf.FriendId == request.FriendId);
            if (exists)
                return BadRequest("Already friends");

            var friend = await _context.Users.FindAsync(request.FriendId);
            if (friend == null)
                return NotFound("Friend not found");

            var userFriend = new UserFriend { UserId = userId, FriendId = request.FriendId };
            _context.UserFriends.Add(userFriend);
            await _context.SaveChangesAsync();
            return Ok("Friend added");
        }

        // DELETE: /api/user/friends/{friendId}
        [HttpDelete("{friendId}")]
        public async Task<IActionResult> RemoveFriend(string friendId)
        {
            var userId = _userManager.GetUserId(User);
            var userFriend = await _context.UserFriends.FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FriendId == friendId);
            if (userFriend == null)
                return NotFound("Friend not found");

            _context.UserFriends.Remove(userFriend);
            await _context.SaveChangesAsync();
            return Ok("Friend removed");
        }
    }

    public class AddFriendRequest
    {
        public string FriendId { get; set; }
    }
}
