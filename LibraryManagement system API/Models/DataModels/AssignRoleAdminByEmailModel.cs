using System.ComponentModel.DataAnnotations;
public class RoleChangeModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } // Email of the user to change role
    [Required(ErrorMessage = "New role is required.")]
    [StringLength(50, ErrorMessage = "Role name cannot be longer than 50 characters.")]
    public string NewRole { get; set; }
}
