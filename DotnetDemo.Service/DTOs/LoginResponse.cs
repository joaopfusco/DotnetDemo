using DotnetDemo.Domain.Models;

namespace DotnetDemo.Service.Dtos
{
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
