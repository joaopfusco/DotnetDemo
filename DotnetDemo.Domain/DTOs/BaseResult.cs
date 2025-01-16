using System;
using System.Collections.Generic;
using DotnetDemo.Domain.Models;

namespace DotnetDemo.Domain.DTOs
{
    public class BaseResult<TModel> where TModel : BaseModel
    {
        public bool Success { get; set; }

        public TModel Data { get; set; }

        public List<string> Errors { get; set; }
    }
}
