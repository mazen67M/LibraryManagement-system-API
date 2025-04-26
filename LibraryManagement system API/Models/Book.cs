using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Book
{
    [Key]
    public int BookId { get; set; }
    [Required]
    [StringLength(255)]
    public string Title { get; set; }
    [Required]
    [StringLength(100)]
    public string Author { get; set; }
    public string Description { get; set; }
    // Foreign Key
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public DateTime PublishedDate { get; set; }
    // Navigation properties
    public virtual Category Category { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
