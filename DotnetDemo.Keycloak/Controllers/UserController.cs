using Microsoft.AspNetCore.Mvc;

namespace DotnetDemo.Keycloak.Controllers
{
    public class UserController(ILogger<UserController> logger) : BaseController(logger)
    {
        [HttpGet("[action]")]
        public IActionResult Me()
        {
            return TryExecute(() =>
            {
                var claims = User.Claims
                    .GroupBy(c => c.Type)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Count() == 1
                            ? (object)g.First().Value
                            : g.Select(c => c.Value).ToList()
                    );

                return Ok(claims);
            });
        }
    }
}
