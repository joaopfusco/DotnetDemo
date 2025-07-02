using DotnetDemo.Service.DTOs;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo.API.Controllers
{
    [AllowAnonymous]
    public class AuthController(IUserService service, ILogger<AuthController> logger) : BaseController(logger)
    {
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginPayload payload)
        {
            return TryExecute(() =>
            {
                var response = service.Authenticate(payload);
                return Ok(response);
            });
        }
    }
}
