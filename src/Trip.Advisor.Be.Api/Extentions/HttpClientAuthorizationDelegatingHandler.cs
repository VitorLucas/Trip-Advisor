using System.Net.Http.Headers;
using Trip.Advisor.Be.Core.Helpers;

namespace  Trip.Advisor.Be.Api.Extentions;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IConfiguration _configuration;
    private readonly OpenAICredential _openAICredential;
    public HttpClientAuthorizationDelegatingHandler(IConfiguration configuration,
                                                    OpenAICredential openAICredential)
    {
        _configuration = configuration;
        _openAICredential = openAICredential;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _openAICredential.APIKey);

        return base.SendAsync(request, cancellationToken);
    }
}
