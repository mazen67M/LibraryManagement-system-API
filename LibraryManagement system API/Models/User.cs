using LibraryManagement_system_API.Models;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    [StringLength(20)]
    public string Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsActive { get; set; } = true;

    [StringLength(15)]
    public string PhoneNumber { get; set; }

    [StringLength(500)]
    public string ProfileImageUrl { get; set; }

    [StringLength(500)]
    public string Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool EmailVerified { get; set; } = false;

    // Relationships
    public ICollection<BorrowedBook> BorrowedBooks { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ICollection<SearchHistory> SearchHistories { get; set; }
    public ICollection<ReadingHistory> ReadingHistories { get; set; }
    public ICollection<Wishlist> Wishlists { get; set; }
    public ICollection<Notification> Notifications { get; set; }
}