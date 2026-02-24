using _2.Application.Entities;
using _3.Infrastructure.Services;
using ArchitecutreMachin.Models.Authentication;
using ArchitecutreMachin.Models.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArchitecutreMachin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(UserManager<User> userManager, JwtService jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                FullName = dto.FullName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null ||
                !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            var token = _jwt.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RoleDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null) return NotFound("User not found");

            if (!await _roleManager.RoleExistsAsync(dto.RoleName))
                await _roleManager.CreateAsync(new IdentityRole(dto.RoleName));

            if (await _userManager.IsInRoleAsync(user, dto.RoleName))
                return BadRequest("User already has this role");

            await _userManager.AddToRoleAsync(user, dto.RoleName);
            return Ok($"Role '{dto.RoleName}' assigned to '{user.UserName}'");
        }

        [HttpGet("my-roles")]
        public async Task<IActionResult> GetMyRoles()
        {
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var user = await _userManager.FindByNameAsync(username);
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);
        }

        [HttpDelete("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null) return NotFound("User not found");

            if (!await _userManager.IsInRoleAsync(user, dto.RoleName))
                return BadRequest("User does not have this role");

            await _userManager.RemoveFromRoleAsync(user, dto.RoleName);
            return Ok($"Role '{dto.RoleName}' removed from '{user.UserName}'");
        }
    }
}
