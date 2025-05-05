using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement_system_API.Models.DTOs;
using LibraryManagement_system_API.Models;
[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly AppDbContext _context;
    public BookController(AppDbContext context)
    {
        _context = context;
    }
    // POST: api/book
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto bookDto)
    {
        if (bookDto == null)
        {
            return BadRequest("Book data is required.");
        }
        // Check if the category exists
        if (!await _context.Categories.AnyAsync(c => c.CategoryId == bookDto.CategoryId))
        {
            return BadRequest("The specified category does not exist.");
        }
        // Create the Book entity
        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            Description = bookDto.Description,
            CategoryId = bookDto.CategoryId, // Associate with the specified category
            PublishedDate = bookDto.PublishedDate, // Set published date
            ImageURL = bookDto.ImageUrl,
        };
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        bookDto.BookId = book.BookId; // Set the BookId from the entity
        return CreatedAtAction(nameof(GetBookById), new { id = book.BookId }, bookDto);
    }

    // GET: api/book/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBookById(int id)
    {
        var book = await _context.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.BookId == id);
        if (book == null)
        {
            return NotFound("Book not found.");
        }
        var bookDto = new BookDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            Description= book.Description,
            CategoryId = book.CategoryId,
            PublishedDate = book.PublishedDate,
            CategoryName = book.Category.CategoryName,
        };
        return Ok(bookDto);
    }
    // GET: api/book
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
    {
        var books = await _context.Books.Include(b => b.Category).ToListAsync();
        // Map entities to DTOs
        var bookDtos = books.Select(book => new BookDto
        {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            CategoryName = book.Category.CategoryName, // Assuming Category has a CategoryName property
           Description = book.Description,
            PublishedDate = book.PublishedDate
            ,CategoryId = book.CategoryId,
        }).ToList();
        return Ok(bookDtos); // Return the list of books as DTOs
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var Book = await _context.Books.FindAsync(id);
        if (Book == null)
        {
            return NotFound("Book not found.");
        }
        _context.Books.Remove(Book);
        _context.SaveChanges();
        return NoContent();
    }
    private bool BookExixts(int id) {
        return _context.Books.Any(e=>e.BookId == id);
    }
}
