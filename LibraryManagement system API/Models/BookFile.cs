using System.ComponentModel.DataAnnotations;

public class BookFile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; }

    [Required]
    [StringLength(500)]
    public string FileUrl { get; set; }

    [Required]
    [StringLength(50)]
    public string FileType { get; set; }

    public long FileSize { get; set; }  // In bytes

}
