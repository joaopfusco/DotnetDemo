using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace DotnetDemo.Filters
{
    public class CustomOperationFilter : IOperationFilter
    {
        private bool RequiresAuthorization(OperationFilterContext context)
        {
            var controller = context.MethodInfo.DeclaringType;
            var action = context.MethodInfo;

            if (controller?.GetCustomAttributes<AllowAnonymousAttribute>().Any() == true ||
                action.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return false;
            }

            bool hasAuthorizeInController = controller?.GetCustomAttributes<AuthorizeAttribute>().Any() == true;
            bool hasAuthorizeInAction = action.GetCustomAttributes<AuthorizeAttribute>().Any();
            bool hasAuthorizeFilter = context.ApiDescription.ActionDescriptor.FilterDescriptors
                .Select(filterInfo => filterInfo.Filter)
                .OfType<AuthorizeFilter>()
                .Any();

            return hasAuthorizeInController || hasAuthorizeInAction || hasAuthorizeFilter;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!RequiresAuthorization(context))
                return;

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var jwtBearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement { [jwtBearerScheme] = new string[] { } }
            };
        }
    }
}
