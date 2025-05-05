namespace LibraryManagement_system_API.Models.DTOs
{
    public class UserDto
    {
        public string UserId { get; set; } // Identifier for the user
        public string FullName { get; set; } // Full name of the user
        public string Email { get; set; } // Email address of the user
        public string PhoneNumber { get; set; } // User's phone number
    }

}
