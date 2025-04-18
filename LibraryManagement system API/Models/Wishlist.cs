using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Wishlist
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    [ForeignKey("Book")]
    public int BookId { get; set; }
    public Book Book { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.Now;
}