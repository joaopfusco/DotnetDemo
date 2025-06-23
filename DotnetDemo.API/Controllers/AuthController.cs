using DotnetDemo.Domain.DTOs;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using DotnetDemo.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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

        [HttpGet("[action]")]
        public IActionResult LoginExternal()
        {
            return TryExecute(() =>
            {
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = "/api/Auth/LoginCallback"
                }, OpenIdConnectDefaults.AuthenticationScheme);
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> LoginCallback()
        {
            return await TryExecuteAsync(async () =>
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (result.Succeeded)
                {
                    var claims = result.Principal.Claims;
                    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    if (email == null)
                        return Unauthorized();

                    var response = service.AuthenticateEmail(email);
                    return Ok(response);
                }

                return Unauthorized();
            });
        }
    }
}
