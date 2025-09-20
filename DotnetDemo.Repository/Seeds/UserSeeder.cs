using DotnetDemo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetDemo.Repository.Seeds
{
    public class UserSeeder
    {
        public static void Seed(ModelBuilder builder)
        {
            var userId = Guid.Parse("f5ff2843-ff7e-454e-9c79-512e5bbfac7c");
            var passwordId = Guid.Parse("beee1b0c-9012-4c89-885e-d0e20f51d71d");

            builder.Entity<User>().HasData(new User
            {
                Id = userId,
                Username = "root",
                Email = "root@email.com"
            });

            builder.Entity<UserPassword>().HasData(new UserPassword
            {
                Id = passwordId,
                UserId = userId,
                Password = "AQAAAAIAAYagAAAAEMRXoBWD0dV14KPMfTS7/OHrK/3OqmT2yvV0nvDFenlQOw7X3wLD3DdBkkMP3SGVFw==",
            });
        }
    }
}
