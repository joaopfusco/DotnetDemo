using DotnetDemo.Service.Services;
using DotnetDemo.Service.Interfaces;

namespace DotnetDemo.API.Extensions
{
    internal static class ServicesExtensions
    {
        internal static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
