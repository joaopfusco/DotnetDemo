using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetDemo.Domain.DTOs
{
    public class BaseResult<TModel>
    {
        public bool Success { get; set; } = true;

        public TModel Data { get; set; }

        public List<string> Errors { get; set; }

        public string Message { get; set; }
    }
}
