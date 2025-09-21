using DotnetDemo.API.Controllers.Abstracts;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo.API.Controllers.Api
{
    public class UserController(IUserService service, IValidator<User> validator, ILogger<UserController> logger) : CrudController<User>(service, validator, logger)
    {
        [HttpGet("[action]")]
        public IActionResult Me()
        {
            return TryExecute(() =>
            {
                var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
                return Ok(claims);
            });
        }
    }
}
