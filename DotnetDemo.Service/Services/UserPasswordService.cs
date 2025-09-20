using Microsoft.AspNetCore.Identity;
using DotnetDemo.Service.Interfaces;
using DotnetDemo.Repository.Data;
using DotnetDemo.Domain.Models;

namespace DotnetDemo.Service.Services
{
    public class UserPasswordService(AppDbContext db) : BaseService<UserPassword>(db), IUserPasswordService
    {
        private readonly PasswordHasher<UserPassword> _passwordHasher = new();

        private string HashPassword(UserPassword model, string password)
        {
            return _passwordHasher.HashPassword(model, password);
        }

        public override async Task<int> Insert(UserPassword model)
        {
            model.Password = HashPassword(model, model.Password);
            return await base.Insert(model);
        }

        public override Task<int> Update(UserPassword model)
        {
            throw new Exception("Invalid action");
        }

        public bool VerifyPassword(UserPassword userPassword, string password)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(userPassword, userPassword.Password, password);
            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}
