using FluentValidation;
using System.Reflection;

namespace DotnetDemo.API.Extensions
{
    internal static class ValidatorsExtensions
    {
        internal static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
