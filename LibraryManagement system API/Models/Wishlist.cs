﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Wishlist
{
    [Key]
    public int WishlistId { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; } // Should match the primary key type of the User entity
    [ForeignKey("Book")]
    public int BookId { get; set; } // Assuming you have a Book entity
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    // Navigation properties
    public virtual User User { get; set; }
    public virtual Book Book { get; set; }
}
