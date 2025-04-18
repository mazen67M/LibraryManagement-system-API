using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_system_API.Models
{
    public class ReadingHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime LastReadAt { get; set; }

        [Range(0, 100)]
        public int ProgressPercent { get; set; }
    }
}
