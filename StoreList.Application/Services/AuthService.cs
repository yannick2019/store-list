using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StoreList.Application.Interfaces;
using StoreList.Infrastructure.Identity;
using StoreList.Domain.Entities;

namespace StoreList.Application.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<(bool Succeeded, string Token, IEnumerable<string> Errors)> RegisterAsync(AddOrUpdateAppUserModel model)
        {
            var existedUser = await _userManager.FindByNameAsync(model.UserName);
            if (existedUser != null)
            {
                return (false, string.Empty, new List<string> { "User name is already taken" });
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return (false, string.Empty, result.Errors.Select(e => e.Description));
            }

            var token = GenerateToken(user);
            return (true, token, Enumerable.Empty<string>());
        }

        public async Task<(bool Succeeded, string Token, IEnumerable<string> Errors)> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return (false, string.Empty, new List<string> { "Invalid username or password" });
            }

            var token = GenerateToken(user);
            return (true, token, Enumerable.Empty<string>());
        }

        private string GenerateToken(AppUser user)
        {
            var secret = _configuration["JwtConfig:Secret"];
            var issuer = _configuration["JwtConfig:ValidIssuer"];
            var audience = _configuration["JwtConfig:ValidAudiences"];

            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new ApplicationException("Jwt configuration is missing");
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
