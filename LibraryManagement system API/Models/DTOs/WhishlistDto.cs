namespace LibraryManagement_system_API.Models.DTOs
{
    public class WishlistDto
    {
        public int WishlistId { get; set; } // Identifier for the wishlist entry
        public int UserId { get; set; } // ID of the user who owns the wishlist
        public int BookId { get; set; } // ID of the book in the wishlist
        public DateTime AddedAt { get; set; } // Timestamp for when the book was added
    }

}
