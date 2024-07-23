using Trip.Advisor.Be.Api.ApiKeyAuth;
using Trip.Advisor.Be.Api.Extentions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using System.Text.Json.Serialization;
using Trip.Advisor.Be.Core.Helpers;

namespace  Trip.Advisor.Be.Api.Configurantions;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddCors(options =>
        {
            options.AddPolicy("Development",
                builder =>
                    builder
                        .WithOrigins()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithHeaders(HeaderNames.ContentType, "Access-Control-Allow-Origin"));

            options.AddPolicy("Staging",
                builder =>
                    builder
                        .WithOrigins()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithHeaders(HeaderNames.ContentType, "Access-Control-Allow-Origin"));

            options.AddPolicy("Production",
                builder =>
                    builder
                        .WithOrigins()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithHeaders(HeaderNames.ContentType, "Access-Control-Allow-Origin"));
        });

        var openAISetting = configuration.GetSection("OpenAICredential").Get<OpenAICredential>();
        services.AddSingleton(openAISetting);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiKeyPolicy", policy =>
            {
                policy.AddAuthenticationSchemes(new[] { JwtBearerDefaults.AuthenticationScheme });
                policy.Requirements.Add(new ApiKeyRequirement());
            });
        });

        return services;
    }

    public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsStaging())
        {
            app.UseCors("Development");
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseCors("Production");
            app.UseAntiXssMiddleware();
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
