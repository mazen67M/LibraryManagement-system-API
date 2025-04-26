using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Review
{
    [Key]
    public int ReviewId { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; } // Should match the primary key type of the User entity
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // Navigation property
    public virtual User User { get; set; }
}
