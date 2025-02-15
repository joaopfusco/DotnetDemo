using DotnetDemo.Domain.DTOs;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Net;

namespace DotnetDemo.Controllers
{
    public class UserController(IUserService service, ILogger<UserController> loger) : BaseController<User>(service, loger)
    {
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginPayload payload)
        {
            try
            {
                var response = service.Authenticate(payload);
                return Ok(response);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
