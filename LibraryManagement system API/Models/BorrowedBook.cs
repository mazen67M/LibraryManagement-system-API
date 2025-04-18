using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_system_API.Models
{
    public class BorrowedBook
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime BorrowDate { get; set; }

        public DateTime ReturnDate { get; set; }

        public DateTime? ReturnedAt { get; set; }

        public bool IsLate { get; set; }

        public decimal? FineAmount { get; set; }
    }
}
