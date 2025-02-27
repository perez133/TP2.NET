// File: Gauniv.WebServer/Api/AdminCategoriesController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gauniv.WebServer.Api
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize(Roles = "Admin")]
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /api/admin/categories
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory([FromBody] AddCategoryRequest request)
        {
            var category = new Category { Nom = request.Nom };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // PUT: /api/admin/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            category.Nom = request.Nom;
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        // DELETE: /api/admin/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: /api/admin/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }
    }

    public class AddCategoryRequest
    {
        public string Nom { get; set; }
    }

    public class UpdateCategoryRequest
    {
        public string Nom { get; set; }
    }
}
