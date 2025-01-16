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
    public class UserController : BaseController<User>
    {
        private new readonly IUserService _service;

        public UserController(IUserService service, ILogger<UserController> loger) : base(service, loger)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] User payload)
        {
            try
            {
                var response = _service.Authenticate(payload);
                return Ok(response);
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }
    }
}
