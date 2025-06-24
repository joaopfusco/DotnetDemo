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
    public class UserController(IUserService service, ILogger<UserController> logger) : CrudController<User>(service, logger)
    {
        [HttpGet("[action]")]
        public IActionResult Me()
        {
            return TryExecute(() =>
            {
                var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
                return Ok(claims);
            });
        }
    }
}
