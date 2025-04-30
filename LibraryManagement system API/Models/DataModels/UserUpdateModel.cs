using System.ComponentModel.DataAnnotations;
public class UserUpdateModel
{
    [Required(ErrorMessage = "User ID is required.")]
    public string UserId { get; set; } // ID of the user being updated

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
    public string FullName { get; set; } // User's full name

    [Phone(ErrorMessage = "Invalid phone number format.")]
    [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters.")]

    public string PhoneNumber { get; set; } // Optional phone number
}
