using System.Text.Json.Serialization;

namespace DotnetDemo.Domain.Models
{
    public class RefreshToken : BaseModel
    {
        public string Token { get; set; }
        public string Identifier { get; set; } = Guid.NewGuid().ToString();
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
