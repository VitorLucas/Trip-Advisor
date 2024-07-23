using Microsoft.Extensions.Logging;
using System.Net;
using Trip.Advisor.Be.Core.Helpers;
using Trip.Advisor.Be.Core.Notifications;
using Trip.Advisor.Be.Domain.Arguments.OpenIA;
using Trip.Advisor.Be.Domain.Arguments.Trip;

namespace Trip.Advisor.Be.Application.Services.OpenIa;

public class PromptService : ServiceBase, IPromptService
{
    private readonly ILogger<PromptService> _logger;
    private readonly OpenAICredential _openAICredential;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string RESPONSE_FORMATE = "{\"destiny\":\"\",\"period\":\"\",\"amount\":{\"accommodation\":0,\"restaurants\":0},\"days\":[{\"day\":1, \"activities\":[{\"name\":\"\",\"detail\":\"\"}]}]}";

    public PromptService(ILogger<PromptService> logger,
                        OpenAICredential openAICredential,
                        IHttpClientFactory httpClientFactory,
                        INotificator notificator) : base(notificator)
    {
        _logger = logger;
        _openAICredential = openAICredential;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result> TriggerOpenAIAsync(TripRequest tripRequest)
    {
        var request = CreateRequest(tripRequest, _openAICredential.Model);
        var openAIResponse = await _httpClientFactory.CreateClient(nameof(PromptService))
                                                        .PostAsync(string.Empty, GetContent(request));

        if (openAIResponse.StatusCode.Equals(HttpStatusCode.OK))
        {
            var response = await DeserializarObjetoResponse<OpenAIResponse>(openAIResponse);

            string tripCreated = response?.choices?.FirstOrDefault()?.message?.content;
            if (string.IsNullOrEmpty(tripCreated))
                return new Result(tripCreated, HttpStatusCode.NotFound, false);

            return new Result(tripCreated, HttpStatusCode.OK);
        }
        else
        {
            var response = DeserializarObjetoResponse<OpenAIErrorResponse>(openAIResponse);
            return new Result(null, openAIResponse.StatusCode, false);
        }
    }

    private OpenAIRequest CreateRequest(TripRequest tripRequest, string model)
    {
        string messageToBeSend = $"Plan a {tripRequest.DaysToTravel} days trip to {tripRequest.CountryDestiny} in with limited cost.Consider accessibles and beautiful destinies. Analyse options based in hosting from {tripRequest.HostAmoutPerNight} euros per night and restaurants with price of {tripRequest.RestaurantsPricePerDay} euros. Return response as json {RESPONSE_FORMATE}";

        return new OpenAIRequest
        {
            Model = model,
            ResponseFormat = new ResponseFormat { Type = "json_object" },
            Messages = new List<OpenAIMessageRequest>{
                new OpenAIMessageRequest
                    {
                        Role = "system",
                        Content = "You are a helpful assistant designed to output JSON."
                    },
                new OpenAIMessageRequest
                    {
                        Role = "user",
                        Content = messageToBeSend
                    }
                },
            MaxTokens = 1000
        };
    }
}