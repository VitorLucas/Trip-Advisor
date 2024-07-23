using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Trip.Advisor.Be.Application.Services.Advisor;
using Trip.Advisor.Be.Core.Helpers;
using Trip.Advisor.Be.Core.Notifications;
using Trip.Advisor.Be.Domain.Arguments.Trip;
using Trip.Advisor.Be.Domain.ViewModels.Finder;

namespace  Trip.Advisor.Be.Api.V1.Controller;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/trip-advisor")]
public class TripAdvisorController : MainController
{
    private readonly IAdvisorService _advisorService;
    private readonly ILogger<TripAdvisorController> _logger;

    public TripAdvisorController(IAdvisorService advisorService,
                               ILogger<TripAdvisorController> logger,
                               INotificator notificator)
                               : base(notificator)
    {
        _advisorService = advisorService;
        _logger = logger;
    }

    [HttpGet()]
    [Authorize(Policy = "ApiKeyPolicy")]
    [SwaggerOperation(summary: "Get advice for a trip")]
    public async Task<IActionResult> AdviceMeAsync([FromQuery] TripViewModel tripModel)
    {
        if (!ModelState.IsValid)
        {
            NotifyError(ModelState);
            var badRequestResult = new Result(tripModel, System.Net.HttpStatusCode.BadRequest);

            return CustomResponse(badRequestResult);
        }

        return CustomResponse(await _advisorService.CreateTripAdviceAsync((TripRequest)tripModel));
    }
}
