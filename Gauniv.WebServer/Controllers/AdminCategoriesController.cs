// File: Gauniv.WebServer/Controllers/AdminCategoriesController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: /AdminCategories/Edit/5
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
            return RedirectToAction(nameof(Index));
        }

        // GET: /AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: /AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
