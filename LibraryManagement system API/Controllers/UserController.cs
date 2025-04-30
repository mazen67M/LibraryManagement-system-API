using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using MimeKit;
using MailKit.Security;
using System.Net;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly EmailService _emailService; // Add EmailService
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserController(UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService; // Injecting EmailService
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors
        }
        if (await _userManager.FindByEmailAsync(model.Email) != null)
        {
            return BadRequest("Email is already registered.");
        }
        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            FullName = model.FullName // Ensure FullName is set here
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return Ok(new { Message = "User registered successfully!" });
        }
        return BadRequest(result.Errors); // Return errors from UserManager
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors if the model state is invalid
        }
        // Attempt to sign the user in with the provided email and password
        var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            // If the login was successful
            return Ok(new { Message = "Login successful!" });
        }
        else if (result.IsLockedOut)
        {
            // If the user account is locked out
            return Unauthorized("User account is locked out.");
        }
        else
        {
            // Invalid login
            return Unauthorized("Invalid email or password.");
        }
    }


    [HttpPost("forgotpassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Ok(new { Message = "If an account with that email exists, a password reset link will be sent." });
        }
        // Generate reset token
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Encode the token to ensure it is URL-safe
        var encodedToken = WebUtility.UrlEncode(token);
        var resetLink = Url.Action("ResetPassword", "User", new { token = encodedToken, email = model.Email }, Request.Scheme);
        // Send the email
        await _emailService.SendEmailAsync(model.Email, "Password Reset Request",
                                            $"Reset your password using this link: {resetLink}");
        return Ok(new { Message = "If an account with that email exists, a password reset link will be sent." });
    }


    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return BadRequest(new { Message = "User not found." }); // Do not disclose user existence to avoid leaks
        }
        // Attempt to reset the password 
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
        {
            return Ok(new { Message = "Password reset successful." });
        }
        // Return errors if reset fails
        return BadRequest(result.Errors);
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        // Fetch all users from the database
        var users = _userManager.Users.ToList(); // Retrieves all users
        return Ok(users); // Return list of users
    }

    //get user by email
    // GET: api/user/email/{email}
    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        // Validate email format if necessary
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required.");
        }
        // Retrieve the user by email
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return NotFound($"No user found with email {email}.");
        }
        return Ok(user); // Return the user information
    }

    //Update user profile
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Return validation errors
        }
        // Retrieve the user using UserManager
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        // Update user properties
        user.FullName = model.FullName; // Assuming FullName is in your User class
        user.PhoneNumber = model.PhoneNumber; // Update phone number if it's provided
        // Update the user in the database
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok(new { Message = "User updated successfully." });
        }
        return BadRequest(result.Errors);
    }


    //Delete User by email
    [HttpDelete("email/{email}")]
    public async Task<IActionResult> DeleteUserByEmail(string email)
    {
        // Validate email format if necessary
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required.");
        }
        // Retrieve the user by email
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound($"No user found with email {email}.");
        }
        // Delete the user
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return NoContent(); // 204 No Content
        }
        // Return any errors during the deletion process
        return BadRequest(result.Errors);
    }

     [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!User.Identity.IsAuthenticated)
        {
            // User is not logged in
            return BadRequest(new { Message = "You need to be logged in to log out." });
        }
        await _signInManager.SignOutAsync(); // Logout the user
        return Ok(new { Message = "Logged out successfully." }); // Return success message
    }

    //Assign Roles to be admin or no
    [HttpPost("assignrole")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRoleToUserByEmail([FromBody] RoleChangeModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        if (!await _roleManager.RoleExistsAsync(model.NewRole))
        {
            return BadRequest("Role does not exist.");
        }
        var result = await _userManager.AddToRoleAsync(user, model.NewRole);
        if (result.Succeeded)
        {
            return Ok(new { Message = "Role assigned successfully." });
        }
        return BadRequest(result.Errors);
    }
}

