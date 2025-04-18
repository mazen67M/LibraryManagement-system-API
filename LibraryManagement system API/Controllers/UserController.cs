using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagement_system_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        AppDbContext context;
        public UserController(AppDbContext _Context)
        {
            context = _Context;
        }
        [HttpGet]
        public IActionResult GetAllUser()
        {
            List<User> users = context.Users.ToList();
            return Ok(users);
        }
    }
}
