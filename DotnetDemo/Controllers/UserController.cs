using DotnetDemo.Domain.DTOs;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DotnetDemo.Controllers
{
    public class UserController(IUserService service, ILogger<UserController> logger) : BaseController<User>(service, logger)
    {
        [AllowAnonymous]
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
