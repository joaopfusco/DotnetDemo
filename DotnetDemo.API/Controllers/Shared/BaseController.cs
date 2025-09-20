using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo.API.Controllers.Shared
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BaseController(ILogger logger) : ControllerBase
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
