using LibraryManagement_system_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
}
