using DotnetDemo.API.Controllers.Abstracts;
using DotnetDemo.Service.DTOs;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo.API.Controllers.Api
{
    [AllowAnonymous]
    public class AuthController(IUserService service, ILogger<AuthController> logger) : BaseController(logger)
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginPayload payload)
        {
            return await TryExecuteAsync(async () =>
            {
                var response = await service.Authenticate(payload);
                return Ok(response);
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Refresh([FromBody] RefreshPayload payload)
        {
            return await TryExecuteAsync(async () =>
            {
                var response = await service.Refresh(payload.RefreshToken);
                return Ok(response);
            });
        }
    }
}
