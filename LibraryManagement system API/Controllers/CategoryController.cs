using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement_system_API.Models; // Adjust the namespace accordingly
using LibraryManagement_system_API.Models.DTOs;
using LibraryManagement_system_API.Models.DataModels; // Include the DTO
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;
    public CategoryController(AppDbContext context)
    {
        _context = context;
    }
    // POST: api/category
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto categoryDto)
    {
        if (categoryDto == null)
        {
            return BadRequest("Category data is required.");
        }
        var category = new Category
        {
            CategoryName = categoryDto.CategoryName,
            Description = categoryDto.Description
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        categoryDto.CategoryId = category.CategoryId; // Set the ID of the created category
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, categoryDto);
    }
    // GET: api/category/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }
        var categoryDto = new CategoryDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        };
        return Ok(categoryDto);
    }
    // GET: api/category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        var categoryDtos = categories.Select(category => new CategoryDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        }).ToList();
        return Ok(categoryDtos);
    }
    // PUT: api/category/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            return BadRequest("Category ID mismatch.");
        }
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }
        // Update category properties
        category.CategoryName = categoryDto.CategoryName;
        category.Description = categoryDto.Description;
        _context.Entry(category).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound("Category not found.");
            }
            throw; // Re-throw the exception
        }
        return NoContent(); // Successfully updated
    }

    // DELETE: api/category/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return NoContent(); // Successfully deleted
    }
    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.CategoryId == id);
    }

    [HttpGet("Count")]
    public ActionResult<List<BookCategoriesCount>> GetCategoriesDetails()
    {
        List<Category> categories =
            _context.Categories.Include(d=>d.Books).ToList();

        List<BookCategoriesCount> BooksCategoriesCount
            = new List<BookCategoriesCount>();

        foreach (var item in categories)
        {
            BookCategoriesCount booksCatCount =
                new BookCategoriesCount();
            booksCatCount.Id = item.CategoryId;
            booksCatCount.Name = item.CategoryName;
            booksCatCount.BooksCount = item.Books.Count();

            BooksCategoriesCount.Add(booksCatCount);
        }
        return Ok(BooksCategoriesCount);
    }
}
