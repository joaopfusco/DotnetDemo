using DotnetDemo.Domain.Models;
using FluentValidation;

namespace DotnetDemo.API.Validators
{
    public class UserPasswordValidator : AbstractValidator<UserPassword>
    {
        public UserPasswordValidator()
        {
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.UserId).NotEmpty();
        }
    }
}
