using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Models;

namespace Gauniv.WebServer.Controllers
{
    [Authorize]
    [Route("Friend")]
    public class FriendController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public FriendController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Friend or /Friend/Index?query=...
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string query = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            // Accepted friends.
            var acceptedFriends = await _context.UserFriends
                .Where(uf => uf.IsAccepted && (uf.UserId == currentUser.Id || uf.FriendId == currentUser.Id))
                .Include(uf => uf.User)
                .Include(uf => uf.Friend)
                .ToListAsync();

            var friends = acceptedFriends
                .Select(uf => uf.UserId == currentUser.Id ? uf.Friend : uf.User)
                .ToList();

            // Pending requests.
            var requests = await _context.UserFriends
                .Where(uf => uf.FriendId == currentUser.Id && !uf.IsAccepted)
                .Include(uf => uf.User)
                .ToListAsync();

            // If a search query is provided, search users by username (case-insensitive).
            var searchResults = Enumerable.Empty<User>();
            if (!string.IsNullOrWhiteSpace(query))
            {
                searchResults = await _context.Users
                    .Where(u => u.UserName.ToLower().Contains(query.ToLower()) && u.Id != currentUser.Id)
                    .ToListAsync();
            }

            var model = new FriendPageViewModel
            {
                Friends = friends,
                Requests = requests,
                SearchResults = searchResults,
                SearchQuery = query
            };

            return View(model);
        }

        // POST: /Friend/Search
        [HttpPost("Search")]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string query)
        {
            // Redirect to GET Index so that all sections display on one page.
            return RedirectToAction("Index", new { query });
        }

        // POST: /Friend/SendRequest
        [HttpPost("SendRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendRequest(string friendId, string query)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index", new { query });
            }

            if (string.IsNullOrEmpty(friendId))
            {
                TempData["Error"] = "Invalid user.";
                return RedirectToAction("Index", new { query });
            }

            var existingRequest = await _context.UserFriends.FirstOrDefaultAsync(uf =>
                (uf.UserId == currentUser.Id && uf.FriendId == friendId) ||
                (uf.UserId == friendId && uf.FriendId == currentUser.Id));

            if (existingRequest != null)
            {
                TempData["Error"] = "Friend request already exists or you are already friends.";
                return RedirectToAction("Index", new { query });
            }

            var friendRequest = new UserFriend
            {
                UserId = currentUser.Id,
                FriendId = friendId,
                IsAccepted = false
            };

            _context.UserFriends.Add(friendRequest);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Friend request sent.";
            return RedirectToAction("Index", new { query });
        }

        // POST: /Friend/Accept
        [HttpPost("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(string requesterId, string query)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var request = await _context.UserFriends.FirstOrDefaultAsync(uf =>
                uf.UserId == requesterId && uf.FriendId == currentUser.Id && !uf.IsAccepted);

            if (request != null)
            {
                request.IsAccepted = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Friend request accepted.";
            }
            else
            {
                TempData["Error"] = "Friend request not found.";
            }
            return RedirectToAction("Index", new { query });
        }

        // POST: /Friend/Decline
        [HttpPost("Decline")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decline(string requesterId, string query)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var request = await _context.UserFriends.FirstOrDefaultAsync(uf =>
                uf.UserId == requesterId && uf.FriendId == currentUser.Id && !uf.IsAccepted);

            if (request != null)
            {
                _context.UserFriends.Remove(request);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Friend request declined.";
            }
            else
            {
                TempData["Error"] = "Friend request not found.";
            }
            return RedirectToAction("Index", new { query });
        }
    }
}
