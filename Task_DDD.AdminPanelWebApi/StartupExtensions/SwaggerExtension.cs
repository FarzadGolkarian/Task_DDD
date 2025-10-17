using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace Task_DDD.AdminPanelWebApi.StartupExtensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddCustomizedSwagger(this IServiceCollection services, IWebHostEnvironment env)
    {
        //if (env.IsDevelopment())

        services.AddSwaggerGen(setupAction =>
        {
            setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization",
            });
            setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }
}
