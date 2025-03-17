using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProductsWebAPI.Models;

namespace ProductsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _key = "ThisIsAReallyStrongSecretKeyWithMoreThan32Chars!!"; // Should be stored in appsettings.json

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            // In a real app, validate the username/password from a database
            if (user.Username == "admin" && user.Password == "password")
            {
                var token = GenerateJwtToken(user.Username);
                return Ok(new { token });
            }
            return Unauthorized("Invalid credentials");
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Name, username),
            //    new Claim(ClaimTypes.Role, "Admin") // You can set roles for authorization
            //};

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7178",
                audience: "https://localhost:7178",
                //claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
