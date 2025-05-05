using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_system_API.Models.DTOs
{
    public class BookDto
    {
        public int BookId { get; set; } // Identifier for the book

        public string Title { get; set; } // Title of the book

        public string Author { get; set; } // Author of the book

        public string Description { get; set; } // Description of the book

        public DateTime PublishedDate { get; set; } // Date when the book was published

        public string CategoryName { get; set; } // Navigation to Category DTO

        // Foreign key for the category
        [Required]
        public int CategoryId { get; set; } // Foreign key linking to the category
    }
}
