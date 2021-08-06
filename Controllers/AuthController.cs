using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using FoodDelivery.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        private async Task<JwtResponse> CreateToken(User user) {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, _configuration["JWT:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["JWT:Issuer"]),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtResponse(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<JwtResponse>> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                return await CreateToken(user);
            }
            return Unauthorized();
        }

        [HttpGet("Refresh")]
        [Authorize]
        public async Task<ActionResult<JwtResponse>> Refresh()
        {
            var user = await userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null) return Unauthorized(new ProblemDetails() { Title = "User not found" });
            return await CreateToken(user);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterModel model)
        {
            if (!(await roleManager.RoleExistsAsync(model.Role)))
                return BadRequest(new ProblemDetails() { Title = "Invalid role" });

            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return Conflict(new ProblemDetails() { Title = "User already exists" });

            User user = new User()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new ProblemDetails() { Title = "Bad username or password" });

            await userManager.AddToRoleAsync(user, model.Role);

            return Ok(new RegisterResponse("Success", "User created successfully"));
        }

        [HttpGet("Unregister")]
        [Authorize]
        public async Task<ActionResult<IdentityResult>> Unregister()
        {
            var user = await userManager.FindByNameAsync(User.Identity?.Name);
            if (user == null) return Unauthorized(new ProblemDetails() { Title = "User not found" });
            return await userManager.DeleteAsync(user);
        }
    }
}
