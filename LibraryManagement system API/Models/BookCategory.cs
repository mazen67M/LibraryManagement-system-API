using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement_system_API.Models
{
    public class BookCategory
    {
        [Key]
        [Column(Order = 1)]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
