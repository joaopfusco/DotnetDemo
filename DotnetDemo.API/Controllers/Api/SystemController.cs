using DotnetDemo.API.Controllers.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace DotnetDemo.API.Controllers.Api
{
    public class SystemController(ILogger<SystemController> logger) : BaseController(logger)
    {
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult Version()
        {
            return TryExecute(() =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                var fileVersionAttribute = entryAssembly?.GetCustomAttribute<AssemblyFileVersionAttribute>();
                var versionNumber = fileVersionAttribute?.Version ?? "Unknown";
                var dateFile = System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd/MM/yyyy");
                return Ok(new { versionNumber, dateFile });
            });
        }
    }
}
