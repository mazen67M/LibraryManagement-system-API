using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; }
    public string Description { get; set; }
    // Navigation property
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
