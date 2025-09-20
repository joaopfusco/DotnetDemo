using DotnetDemo.Domain.Models;
using DotnetDemo.Repository.Data;
using DotnetDemo.Service.DTOs;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace DotnetDemo.Service.Services
{
    public class RefreshTokenService(AppDbContext db) : BaseService<RefreshToken>(db), IRefreshTokenService
    {
        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Base64UrlEncoder.Encode(bytes);

            await Insert(new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                IsRevoked = false
            });

            return token;
        }

        public async Task<Refreshed> RefreshToken(string refreshToken)
        {
            var storedToken = Get(t => t.Token == refreshToken)
                .FirstOrDefault()
                ?? throw new Exception("Invalid refresh token");

            if (storedToken.IsUsed || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token");

            storedToken.IsUsed = true;
            await Update(storedToken);

            var newRefreshToken = await GenerateRefreshToken(storedToken.UserId);
            return new Refreshed 
            { 
                UserId = storedToken.UserId,
                RefreshToken = newRefreshToken,
            };
        }
    }

    public class Refreshed
    {
        public Guid UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
