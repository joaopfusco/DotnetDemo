using DotnetDemo.API.Controllers.Shared;
using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Interfaces;

namespace DotnetDemo.API.Controllers.Api
{
    public class UserPasswordController(IUserPasswordService service, ILogger<UserPasswordController> logger) : CrudController<UserPassword>(service, logger)
    {
    }
}
