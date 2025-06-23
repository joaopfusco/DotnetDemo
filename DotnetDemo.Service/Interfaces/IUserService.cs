using DotnetDemo.Domain.DTOs;
using DotnetDemo.Domain.Models;

namespace DotnetDemo.Service.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        LoginResponse Authenticate(LoginPayload payload);
        LoginResponse AuthenticateEmail(string email);
    }
}
