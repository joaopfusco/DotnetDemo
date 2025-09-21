using DotnetDemo.API.Controllers.Abstracts;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using FluentValidation;

namespace DotnetDemo.API.Controllers.Api
{
    public class UserPasswordController(IUserPasswordService service, IValidator<UserPassword> validator, ILogger<UserPasswordController> logger) : CrudController<UserPassword>(service, validator, logger)
    {
    }
}
