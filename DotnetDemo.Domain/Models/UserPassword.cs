using System.Text.Json.Serialization;

namespace DotnetDemo.Domain.Models
{
    public class UserPassword : BaseModel
    {
        public string Password { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
