using DotnetDemo.Domain.Models;

namespace DotnetDemo.Service.Interfaces
{
    public interface IUserPasswordService : IBaseService<UserPassword>
    {
        bool VerifyPassword(UserPassword userPassword, string password);
    }
}
