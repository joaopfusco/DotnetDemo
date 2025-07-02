using DotnetDemo.Domain.Models;

namespace DotnetDemo.Service.DTOs
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required User User { get; set; }
    }
}
