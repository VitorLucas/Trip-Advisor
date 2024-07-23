using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Trip.Advisor.Be.Api.ApiKeyAuth;
using Trip.Advisor.Be.Application.Services.Advisor;
using Trip.Advisor.Be.Application.Services.OpenIa;
using Trip.Advisor.Be.Core.Notifications;

namespace  Trip.Advisor.Be.Api.Configurantions;

public static class DependencyInjectionConfig
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Application(configuration);
        //services.Repositories();
        services.Services();

        return services;
    }

    private static void Application(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpFactoryConfiguration(configuration);
        services.AddScoped<INotificator, Notificator>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
        services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();
    }

    private static void Services(this IServiceCollection services)
    {
        services.AddScoped<IPromptService, PromptService>();
        services.AddScoped<IAdvisorService, AdvisorService>();
        
    }

    private static void Repositories(this IServiceCollection services)
    {
    }
}
