using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DotnetDemo.Service.Interfaces;
using DotnetDemo.Repository.Data;
using DotnetDemo.Domain.Models;
using DotnetDemo.Domain.DTOs;

namespace DotnetDemo.Service.Services
{
    public class UserService(AppDbContext db, IConfiguration configuration) : BaseService<User>(db), IUserService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly PasswordHasher<User> _passwordHasher = new();

        private string HashedPassword(User user)
        {
            return _passwordHasher.HashPassword(user, user.Password);
        }

        public override async Task<int> Insert(User model)
        {
            model.Password = HashedPassword(model);
            return await base.Insert(model);
        }

        public override async Task<int> Update(User model)
        {
            model.Password = HashedPassword(model);
            return await base.Update(model);
        }

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
            var _user = Get(u => u.Email == payload.Email)
                .FirstOrDefault() ?? throw new Exception("Usuário não existe!");

            var verificationResult = _passwordHasher.VerifyHashedPassword(_user, _user.Password, payload.Password);

            if (verificationResult != PasswordVerificationResult.Success)
                throw new Exception("Usuário ou senha incorretos!");

            var token = GenerateToken(_user);

            return new LoginResponse
            {
                Token = token,
                User = _user,
            };
        }

        public LoginResponse AuthenticateEmail(string email)
        {
            var _user = Get(u => u.Email == email)
                .FirstOrDefault() ?? throw new Exception("Usuário não existe!");

            var token = GenerateToken(_user);

            return new LoginResponse
            {
                Token = token,
                User = _user,
            };
        }
    }
}
