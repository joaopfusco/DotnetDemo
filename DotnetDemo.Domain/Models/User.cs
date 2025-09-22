using System.Text.Json.Serialization;

namespace DotnetDemo.Domain.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
