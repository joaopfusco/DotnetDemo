using DotnetDemo.Domain.Models;
using DotnetDemo.Repository.Data;
using DotnetDemo.Service.DTOs;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace DotnetDemo.Service.Services
{
    public class RefreshTokenService(AppDbContext db) : BaseService<RefreshToken>(db), IRefreshTokenService
    {
        private readonly PasswordHasher<RefreshToken> _passwordHasher = new();

        private string HashToken(RefreshToken model, string token)
        {
            return _passwordHasher.HashPassword(model, token);
        }

        public bool VerifyToken(RefreshToken refreshToken, string token)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(refreshToken, refreshToken.Token, token);
            return verificationResult == PasswordVerificationResult.Success;
        }

        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Base64UrlEncoder.Encode(bytes);

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                IsRevoked = false
            };

            refreshToken.Token = HashToken(refreshToken, token);

            await Insert(refreshToken);

            return $"{refreshToken.Identifier}:{token}";
        }

        public async Task<Refreshed> RefreshToken(string token)
        {
            var parts = token.Split(':');
            if (parts.Length != 2) throw new Exception("Invalid token format");

            var identifier = parts[0];
            var secret = parts[1];

            var storedToken = Get(t => t.Identifier == identifier)
                .FirstOrDefault()
                ?? throw new Exception("Invalid refresh token");

            var validToken = VerifyToken(storedToken, secret);

            if (!validToken || storedToken.IsUsed || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
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
