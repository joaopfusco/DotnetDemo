namespace DotnetDemo.Keycloak.Extensions
{
    internal static class ServicesExtensions
    {
        internal static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            //services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}
