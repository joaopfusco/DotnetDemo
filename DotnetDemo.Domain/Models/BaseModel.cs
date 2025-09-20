using DotnetDemo.Domain.Interfaces;
using System.Text.Json.Serialization;

namespace DotnetDemo.Domain.Models
{
    public class BaseModel : IBaseModel<Guid>
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
