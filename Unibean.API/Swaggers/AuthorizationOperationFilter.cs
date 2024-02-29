using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Unibean.API.Swaggers;

public class AuthorizationOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Get Authorize attribute
        var attributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                .Union(context.MethodInfo.GetCustomAttributes(true))
                                .OfType<AuthorizeAttribute>();

        if (attributes != null && attributes.Any())
        {
            var attr = attributes.ToList()[0];

            // Add response types on secure APIs
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            // Add what should be show inside the security section
            IList<string> securityInfos = new List<string>
            {
                $"{nameof(AuthorizeAttribute.Policy)}:{attr.Policy}",
                $"{nameof(AuthorizeAttribute.Roles)}:{attr.Roles}",
                $"{nameof(AuthorizeAttribute.AuthenticationSchemes)}:{attr.AuthenticationSchemes}"
            };

            operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer", // Must fit the defined Id of SecurityDefinition in global configuration
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            securityInfos
                        }
                    }
                };
        }
    }
}
