using Microsoft.OpenApi.Models;

#nullable disable

namespace DotnetDemo.API.Extensions
{
    internal static class SwaggerGenExtensions
    {
        internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                #region OAuth2
                //c.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        Implicit = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]),
                //            Scopes = new Dictionary<string, string>
                //            {
                //                { "openid", "OpenID Connect scope" }
                //            }
                //        }
                //    }
                //});
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Id = "OAuth2",
                //                Type = ReferenceType.SecurityScheme,
                //            },
                //            In = ParameterLocation.Header,
                //            Name = "Bearer",
                //            Scheme = "Bearer"
                //        },
                //        []
                //    }
                //});
                #endregion

                #region JWT Token
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "`Token apenas!`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme,
                            }
                        },
                        []
                    }
                });
                #endregion
            });

            return services;
        }
    }
}
