using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using Trip.Advisor.Be.Application.Services.OpenIa;
using Trip.Advisor.Be.Core.Extentions;
using Trip.Advisor.Be.Core.Helpers;
using Trip.Advisor.Be.Core.Notifications;
using Trip.Advisor.Be.Domain.Arguments.Trip;

namespace  Trip.Advisor.Be.Application.Services.Advisor
{
    public class AdvisorService : ServiceBase, IAdvisorService
    {
        private readonly ILogger<AdvisorService> _logger;
        private readonly IPromptService _promptService;

        public AdvisorService(ILogger<AdvisorService> logger,
                                IPromptService promptService,
                                INotificator notificator) : base(notificator)
        {
            _logger = logger;
            _promptService = promptService;
        }

        public async Task<Result> CreateTripAdviceAsync(TripRequest tripRequest)
        {
            if (tripRequest.IsNullObject())
            {
                Notify("trip request is null ");
                return new Result(null, HttpStatusCode.BadRequest, false);
            }

            var countryDestiny = FilterByBlackListWords(tripRequest.CountryDestiny);
            if (string.IsNullOrEmpty(countryDestiny))
            {
                Notify($"Country contains words filtered by backlist");
                return new Result(null, HttpStatusCode.BadRequest, false);
            }

            var response = await _promptService.TriggerOpenAIAsync(tripRequest);

            if (!response.Data.IsNullObject() && response.StatusCode.Equals(HttpStatusCode.OK))
            {
                var tripCreated = ConvertOpenIaResponseToTripResponse(response?.Data?.ToString());
                if (!tripCreated.IsNullObject())
                {
                    return new Result(tripCreated, HttpStatusCode.OK);
                }

                return new Result(tripCreated, HttpStatusCode.NotFound);
            }

            return response;
        }

        private TripResponse? ConvertOpenIaResponseToTripResponse(string content)
        {
            try
            {
                return JsonConvert.DeserializeObject<TripResponse>(content);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string FilterByBlackListWords(string words)
        {
            if (string.IsNullOrWhiteSpace(words))
            {
                return words;
            }

            List<string> balcklist = new List<string>() { "crazy"};
            if (balcklist.Contains(words.FormatString()))
            {
                return string.Empty;
            }

            return words;
        }
    }
}
