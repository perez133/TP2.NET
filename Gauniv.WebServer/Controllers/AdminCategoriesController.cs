// File: Controllers/AdminCategoriesController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /AdminCategories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: /AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /AdminCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminCategories/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: /AdminCategories/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            _context.Categories.Update(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminCategories/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: /AdminCategories/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
