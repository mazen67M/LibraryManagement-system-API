namespace LibraryManagement_system_API.Models.DTOs
{
    public class ReviewDto
    {
        public int ReviewId { get; set; } // Identifier for the review
        public string Comment { get; set; } // The review comment
        public int UserId { get; set; } // ID of the user who wrote the review
        public int BookId { get; set; } // ID of the reviewed book
        public DateTime CreatedAt { get; set; } // Timestamp for when the review was created
    }

}
