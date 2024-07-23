using Trip.Advisor.Be.Core.Helpers;
using Trip.Advisor.Be.Domain.Arguments.Trip;

namespace  Trip.Advisor.Be.Application.Services.Advisor;

public interface IAdvisorService
{
    Task<Result> CreateTripAdviceAsync(TripRequest tripRequest);
}