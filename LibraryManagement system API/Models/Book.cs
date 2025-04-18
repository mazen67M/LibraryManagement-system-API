using LibraryManagement_system_API.Models;
using System.ComponentModel.DataAnnotations;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsEbook { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public int TotalCopies { get; set; }

    [StringLength(500)]
    public string OnlineReadingUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [StringLength(500)]
    public string CoverImageUrl { get; set; }

    public DateTime PublishedDate { get; set; }

    [StringLength(200)]
    public string Publisher { get; set; }

    [StringLength(50)]
    public string Language { get; set; }

    public int PageCount { get; set; }

    public double AverageRating { get; set; }

    // Relationships
    public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; }
    public ICollection<BookCategory> BookCategories { get; set; }
    public ICollection<BookFile> BookFiles { get; set; }
    public ICollection<Wishlist> Wishlists { get; set; }
}
