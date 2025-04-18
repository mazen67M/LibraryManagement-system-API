using LibraryManagement_system_API.Models;
using System.ComponentModel.DataAnnotations;

public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    public string Biography { get; set; }

    public DateTime BirthDate { get; set; }

    [StringLength(50)]
    public string Nationality { get; set; }

    [StringLength(500)]
    public string ProfileImageUrl { get; set; }

    // Relationships
    public ICollection<BookAuthor> BookAuthors { get; set; }
}