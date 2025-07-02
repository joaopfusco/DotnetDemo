using Microsoft.AspNetCore.Identity;
using DotnetDemo.Service.Interfaces;
using DotnetDemo.Repository.Data;
using DotnetDemo.Domain.Models;

namespace DotnetDemo.Service.Services
{
    public class UserPasswordService(AppDbContext db) : BaseService<UserPassword>(db), IUserPasswordService
    {
        private readonly PasswordHasher<UserPassword> _passwordHasher = new();

        public string HashedPassword(UserPassword model)
        {
            return _passwordHasher.HashPassword(model, model.Password);
        }

        public override async Task<int> Insert(UserPassword model)
        {
            model.Password = HashedPassword(model);
            return await base.Insert(model);
        }

        public override Task<int> Update(UserPassword model)
        {
            throw new Exception("Ação inválida!");
        }

        public bool VerifyPassword(UserPassword userPassword, string password)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(userPassword, userPassword.Password, password);
            return verificationResult == PasswordVerificationResult.Success;
        }
    }
}
