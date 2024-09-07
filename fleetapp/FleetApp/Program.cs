using Dashdine.Application.Converter;
using Dashdine.Application.Extensions;
using Dashdine.Application.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dashdine.Application;

internal static class Program
{
    private static void Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddResponseCaching();

        builder.Services
            .AddControllers(options =>
            {
                options.CacheProfiles.Add("default",
                    new Microsoft.AspNetCore.Mvc.CacheProfile()
                    {
                        VaryByHeader = "Authorization",
                        Location = Microsoft.AspNetCore.Mvc.ResponseCacheLocation.Client,
                        NoStore = false,
                    });

                options.Filters.Add<ModeloDeValidacaoDeAtributo>(int.MinValue);
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.CustomAddSwaggerGen();

        builder.Services.AddMemoryCache();

        builder.Services.AddDependencies(builder.Configuration);

        #region Seguranca - Servico
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                name: "APIPolicy",
                     policy =>
                     {
                         policy
                         .AllowAnyOrigin()
                         .WithMethods("POST", "GET", "PUT", "DELETE")
                         .AllowAnyHeader();
                     });
        });
        #endregion

        #region Token
        builder.Services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:key"] ?? string.Empty)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });
        #endregion

        builder.Services.AddHealthChecks();

        WebApplication app = builder.Build();

        app.MapHealthChecks("/health");

        app.CustomUseSwaggerUI();

        app.UseRouting();
        #region Seguranca
        app.UseCors("APIPolicy");

        app.Use(async (context, next) =>
        {
            context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = false,
                    Private = true,
                    NoStore = true,
                    MustRevalidate = true,
                    MaxAge = TimeSpan.FromSeconds(10),
                };

            context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                new string[] { "Accept-Encoding" };

            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

            context.Response.Headers.Append(
                     "Content-Security-Policy",
                         "default-src 'self';" +
                         "base-uri 'self';" +
                         "child-src 'none' ;" +
                         "connect-src 'self' ;" +
                         "img-src 'self' data:;" +
                         "font-src 'self' ;" +
                         "form-action 'self' ;" +
                         "frame-ancestors 'self' ;" +
                         "frame-src 'none' ;" +
                         "object-src 'self' ;" +
                         "upgrade-insecure-requests ;" +
                         "worker-src 'self' ;" +

                         //JavaScript -------------------------------------------------->
                         "script-src 'self' " +
                             "https://code.jquery.com/jquery-3.3.1.slim.min.js " +
                             "https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js " +
                             "https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js " +
                             "js/site.js " +
                             //" 'unsafe-inline' " +
                             //" 'unsafe-eval' " +
                             " 'report-sample' " +
                             ";" +

                         //CSS --------------------------------------------------------->
                         "style-src 'self' " +
                             "https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css " +
                             "css/site.css " +
                             " 'report-sample' " +
                             ";" +

                         //Reportar erros seguranca ------------------------------------->
                         "report-uri portal/seguraca ;" +
                         ""
                 );

            await next();
        });

        app.UseResponseCaching();
        //app.UseHttpsRedirection();
        #endregion

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.MapControllers();

        app.Run();
    }
}