using API.v1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.v1.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration configuration;
        private readonly ApiDbContext dbContext;
        private readonly PasswordHasher<Worker> hasher;

        public AuthService(IConfiguration configuration, ApiDbContext dbContext)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.hasher = new PasswordHasher<Worker>();
        }

        public async Task<string?> Authenticate(string username, string password)
        {
            if (await CheckUser(username, password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, username),
                    }),

                    Issuer = configuration["Jwt:Issuer"],
                    Audience = configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    )   
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            return null;
        }

        private async Task<bool> CheckUser(string username, string password)
        {
            var worker = await dbContext.Workers.FirstOrDefaultAsync(w => w.UserName == username);
            if (worker == null)
            {
                return false;
            }

            var passCheck = hasher.VerifyHashedPassword(worker, worker.UserPassword, password);

            return passCheck == PasswordVerificationResult.Success;
        }
    }
}           
