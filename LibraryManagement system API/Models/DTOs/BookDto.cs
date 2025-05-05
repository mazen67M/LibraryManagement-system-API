using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_system_API.Models.DTOs
{
    public class BookDto
    {
        [Required]
        public int BookId { get; set; } // Identifier for the book

        [Required]
        public string Title { get; set; } // Title of the book

        [Required]
        public string Author { get; set; } // Author of the book

        public string Description { get; set; } // Description of the book

        public DateTime PublishedDate { get; set; } // Date when the book was published

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; } // Navigation to Category DTO

        // Foreign key for the category
        [Required]
        public int CategoryId { get; set; } // Foreign key linking to the category
    }
}
