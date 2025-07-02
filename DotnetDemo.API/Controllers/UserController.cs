using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo.API.Controllers
{
    public class UserController(IUserService service, ILogger<UserController> logger) : CrudController<User>(service, logger)
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
