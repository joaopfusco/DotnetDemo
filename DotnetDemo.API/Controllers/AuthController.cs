using DotnetDemo.Domain.DTOs;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using DotnetDemo.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

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
