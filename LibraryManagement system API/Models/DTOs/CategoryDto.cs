using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_system_API.Models.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; } // Identifier for the category
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } // Name of the category
        public string Description { get; set; } // Description of the category
        // You may choose to include other relevant information if needed
    }
}
