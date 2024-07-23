using Trip.Advisor.Be.Core.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Trip.Advisor.Be.Domain.Arguments.Trip;

namespace  Trip.Advisor.Be.Application.Services.OpenIa;

public interface IPromptService
{
    Task<Result> TriggerOpenAIAsync(TripRequest tripRequest);
}