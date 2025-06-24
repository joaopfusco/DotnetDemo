using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotnetDemo.Keycloak.Controllers
{
    [AllowAnonymous]
    public class AuthController(ILogger<AuthController> logger) : BaseController(logger)
    {
        [HttpGet("[action]")]
        public IActionResult Login()
        {
            return TryExecute(() =>
            {
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = "/api/Auth/Callback"
                }, OpenIdConnectDefaults.AuthenticationScheme);
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Callback()
        {
            return await TryExecuteAsync(async () =>
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (!result.Succeeded)
                    return Unauthorized();

                var claims = result.Principal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var accessToken = result.Properties.GetTokenValue("access_token");

                return Ok(new
                {
                    Email = email,
                    AccessToken = accessToken
                });
            });
        }
    }
}
