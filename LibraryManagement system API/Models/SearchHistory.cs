using System.ComponentModel.DataAnnotations;

namespace LibraryManagement_system_API.Models
{
    public class SearchHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(255)]
        public string SearchTerm { get; set; }

        public DateTime SearchedAt { get; set; } = DateTime.Now;
    }
}
