using Microsoft.OpenApi.Models;

namespace Dashdine.Application.Extensions;

public static class SwaggerSetup
{
    public static void CustomAddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Dashdine API",
                Version = "v1",
                Description = "Documentação interna do Dashdine",
                Contact = new OpenApiContact { Email = "dashdineapp@gmail.com", Name = "Dashdine" }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = $"JWT Authorization header using the Bearer scheme. {Environment.NewLine} Enter 'Bearer'[space] and then your token in the text input below."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
        });
    }

    public static void CustomUseSwaggerUI(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty;
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fleet App v1");
        });
    }
}
