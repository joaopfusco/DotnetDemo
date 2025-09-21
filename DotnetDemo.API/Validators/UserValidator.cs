using DotnetDemo.Domain.Models;
using FluentValidation;

namespace DotnetDemo.API.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.Email).EmailAddress().NotEmpty();
        }
    }
}
