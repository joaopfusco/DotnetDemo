using DotnetDemo.Domain.Models;
using DotnetDemo.Service.Services;

namespace DotnetDemo.Service.Interfaces
{
    public interface IRefreshTokenService : IBaseService<RefreshToken>
    {
        Task<string> GenerateRefreshToken(Guid userId);
        Task<Refreshed> RefreshToken(string refreshToken);
    }
}
