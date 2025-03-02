// File: Controllers/FriendController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class FriendController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public FriendController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Friend
        // Display the logged-in player's friend list.
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Eager-load the friends.
            var currentUser = await _context.Users
                .Include(u => u.Friends)
                    .ThenInclude(f => f.Friend)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return View(currentUser.Friends);
        }
    }
}
