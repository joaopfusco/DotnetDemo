using DotnetDemo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetDemo.Domain.DTOs
{
    public class LoginPayload
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
