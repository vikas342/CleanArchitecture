using Identity.Dtos;
using Identity.Services;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IJwtService _jwtService;

        public AuthController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _roleManager = roleManager;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                Role = dto.Role,
                
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            string role = string.IsNullOrEmpty(dto.Role) ? "User" : dto.Role;

            // Verify role exists
            if (!await _roleManager.RoleExistsAsync(role))
                return BadRequest("Role does not exist");

            await _userManager.AddToRoleAsync(user, role);

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            var roles = await _userManager.GetRolesAsync(user);


            if (user == null)
                return Unauthorized("Invalid Email or Password");

            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
                return Unauthorized("Invalid Email or Password");

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                access_token = token,
                email = user.Email,
                role = user.Role
            });
        }
    }
}

