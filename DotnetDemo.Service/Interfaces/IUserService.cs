using DotnetDemo.Domain.Models;
using DotnetDemo.Service.DTOs;

namespace DotnetDemo.Service.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        LoginResponse Authenticate(LoginPayload payload);
    }
}
