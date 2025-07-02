using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DotnetDemo.Service.Interfaces;
using DotnetDemo.Repository.Data;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DotnetDemo.Service.Services
{
    public class UserService(AppDbContext db, IConfiguration configuration, IUserPasswordService userPasswordService) : BaseService<User>(db), IUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserPasswordService _userPasswordService = userPasswordService;

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var keyString = _configuration["JwtKey"] ?? throw new Exception("Key not found in configuration");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public LoginResponse Authenticate(LoginPayload payload)
        {
            var user = Get(u => u.Email == payload.Credential || u.Username == payload.Credential)
                .FirstOrDefault() ?? throw new Exception("Usuário não existe!");

            var userPassword = _userPasswordService.Get(up => up.UserId == user.Id)
                .OrderByDescending(up => up.CreatedAt)
                .FirstOrDefault() ?? throw new Exception("Usuário não possui senha!");

            var verificationResult = _userPasswordService.VerifyPassword(userPassword, payload.Password);
            if (!verificationResult)
                throw new Exception("Senha incorreta!");

            var token = GenerateToken(user);
            user.UserPasswords = null;
            return new LoginResponse
            {
                Token = token,
                User = user,
            };
        }
    }
}
