using DotnetDemo.Domain.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace DotnetDemo.Domain.Models
{
    public class BaseModel : IBaseModel<int>
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime UpdatedAt { get; set; }
    }
}
