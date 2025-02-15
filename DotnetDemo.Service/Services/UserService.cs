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
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext db, IConfiguration configuration) : base(db)
        {
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        private string HashedPassword(User user)
        {
            return _passwordHasher.HashPassword(user, user.Password);
        }

        public override int Insert(User model)
        {
            model.Password = HashedPassword(model);
            return base.Insert(model);
        }

        public override int Update(User model)
        {
            model.Password = HashedPassword(model);
            return base.Update(model);
        }

        public LoginResponse Authenticate(LoginPayload payload)
        {
            var _user = Get(u => u.Username == payload.Username)
                .FirstOrDefault() ?? throw new Exception("Usuário não existe!");

            var verificationResult = _passwordHasher.VerifyHashedPassword(_user, _user.Password, payload.Password);

            if (verificationResult != PasswordVerificationResult.Success)
                throw new Exception("Usuário ou senha incorretos!");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, _user.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: creds);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = _user,
            };
        }
    }
}
