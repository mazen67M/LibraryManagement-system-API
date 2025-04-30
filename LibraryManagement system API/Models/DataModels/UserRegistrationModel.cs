using System.ComponentModel.DataAnnotations;

public class UserRegistrationModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
    [Required] // Make it required if you want to enforce this
    public string FullName { get; set; }
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }
}
