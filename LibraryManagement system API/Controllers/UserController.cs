using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using MimeKit;
using MailKit.Security;
using System.Net;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly EmailService _emailService; // Add EmailService
    public UserController(UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService; // Injecting EmailService
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
}