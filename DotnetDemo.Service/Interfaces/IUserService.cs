using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Dtos;

namespace DotnetDemo.Service.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        Task<LoginResponse> Authenticate(LoginPayload payload);
        Task<LoginResponse> Refresh(string refreshToken);
    }
}
