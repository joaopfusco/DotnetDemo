using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DotnetDemo.Service.Interfaces;
using DotnetDemo.Repository.Data;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DotnetDemo.Service.Services
{
    public class UserService(AppDbContext db, IConfiguration configuration, IUserPasswordService userPasswordService, IRefreshTokenService refreshTokenService) : BaseService<User>(db), IUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserPasswordService _userPasswordService = userPasswordService;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

        private string GenerateAccessToken(User user)
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

        public async Task<LoginResponse> Authenticate(LoginPayload payload)
        {
            var user = Get(u => u.Email == payload.Credential || u.Username == payload.Credential)
                .FirstOrDefault() ?? throw new Exception("Invalid credentials");

            var userPassword = _userPasswordService.Get(up => up.UserId == user.Id)
                .OrderByDescending(up => up.CreatedAt)
                .FirstOrDefault() ?? throw new Exception("Invalid credentials");

            var verificationResult = _userPasswordService.VerifyPassword(userPassword, payload.Password);
            if (!verificationResult)
                throw new Exception("Invalid credentials");

            var accessToken = GenerateAccessToken(user);
            var refreshToken = await _refreshTokenService.GenerateRefreshToken(user.Id);
            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<LoginResponse> Refresh(string refreshToken)
        {
            var refreshed = await _refreshTokenService.RefreshToken(refreshToken);
            var user = Get(refreshed.UserId)
                .FirstOrDefault() ?? throw new Exception("Usuário não encontrado!"); ;

            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = refreshed.RefreshToken;
            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
