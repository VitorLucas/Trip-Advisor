using Trip.Advisor.Be.Api.Extentions;
using Trip.Advisor.Be.Application.Services.OpenIa;
using Trip.Advisor.Be.Core.Helpers;

namespace  Trip.Advisor.Be.Api.Configurantions;

public static class HttpFactoryConfig
{
    public static IServiceCollection AddHttpFactoryConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var credentialsSection = configuration.GetSection("OpenAICredential");
        services.Configure<OpenAICredential>(credentialsSection);

        services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

        services.AddHttpClient<IPromptService, PromptService>(nameof(PromptService), client =>
        {
            client.BaseAddress = new Uri(configuration["OpenAICredential:BaseUrl"]);
        })
        .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

        return services;
    }
}
