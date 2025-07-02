namespace DotnetDemo.Service.DTOs
{
    public class LoginPayload
    {
        public required string Credential { get; set; }
        public required string Password { get; set; }
    }
}
