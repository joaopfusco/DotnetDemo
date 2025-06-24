using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo.Keycloak.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController(ILogger logger) : Controller
    {
        private readonly ILogger _logger = logger;

        [NonAction]
        protected virtual IActionResult TryExecute(Func<IActionResult> execute)
        {
            try
            {
                return execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.InnerException);
                return BadRequest(ex);
            }
        }

        [NonAction]
        protected virtual async Task<IActionResult> TryExecuteAsync(Func<Task<IActionResult>> execute)
        {
            try
            {
                return await execute();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.InnerException);
                return BadRequest(ex);
            }
        }

    }
}
