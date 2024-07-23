using Trip.Advisor.Be.Application._Util;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Trip.Advisor.Be.Application.Services.Advisor;
using Trip.Advisor.Be.Application.Services.OpenIa;
using Trip.Advisor.Be.Core.Notifications;
using Trip.Advisor.Be.Domain.Arguments.Trip;

namespace  Trip.Advisor.Be.Application;

public class AdvisorServiceTest
{
    private readonly Mock<INotificator> _notificatorMock;
    private readonly Mock<ILogger<AdvisorService>> _loggerMock;
    private readonly Mock<IPromptService> _promptServiceMock;

    public AdvisorServiceTest()
    {
        _loggerMock = new Mock<ILogger<AdvisorService>>();
        _promptServiceMock = new Mock<IPromptService>();
        _notificatorMock = new Mock<INotificator>();

    }

    [Fact]
    public async Task CreateTripAdviceAsync_SendTripRequestNull_ReturnBadRequest()
    {
        //Arrange 
        var advisorService = new AdvisorService(_loggerMock.Object,
                                                _promptServiceMock.Object,
                                                _notificatorMock.Object);
        //Act
        var result = await advisorService.CreateTripAdviceAsync(null);

        //Asset
        Assert.Equal(result.StatusCode, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTripAdviceAsync_SendTripRequestCountryDestinyEmpty_ReturnBadRequest()
    {
        //Arrange 

        var tripRequest = new TripRequest(0, string.Empty, 0, 0);

        var advisorService = new AdvisorService(_loggerMock.Object,
                                                _promptServiceMock.Object,
                                                _notificatorMock.Object);
        //Act
        var result = await advisorService.CreateTripAdviceAsync(tripRequest);

        //Asset
        Assert.Equal(result.StatusCode, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateTripAdviceAsync_SendTripRequestCountryDestinyBlackList_ReturnBadRequest()
    {
        //Arrange 

        var tripRequest = new TripRequest(2, "CRAZY", 100, 100);

        var advisorService = new AdvisorService(_loggerMock.Object,
                                                _promptServiceMock.Object,
                                                _notificatorMock.Object);
        //Act
        var result = await advisorService.CreateTripAdviceAsync(tripRequest);

        //Asset
        Assert.Equal(result.StatusCode, HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task CreateTripAdviceAsync_SendTripRequestCountryDestiny_ReturnOk()
    {
        //Arrange 
        var tripRequest = new TripRequest(2, "Lisbon", 100, 100);

        _promptServiceMock.Setup(x => x.TriggerOpenAIAsync(It.IsAny<TripRequest>()))
            .ReturnsAsync(OpenIaResponseMock.CreateOpenIaValidResult());

        var advisorService = new AdvisorService(_loggerMock.Object,
                                                _promptServiceMock.Object,
                                                _notificatorMock.Object);
        //Act
        var result = await advisorService.CreateTripAdviceAsync(tripRequest);

        //Asset
        Assert.Equal(result.StatusCode, HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateTripAdviceAsync_OpenAIResponseEmpty_ReturnNotFound()
    {
        //Arrange 
        var tripRequest = new TripRequest(2, "Lisbon", 100, 100);

        _promptServiceMock.Setup(x => x.TriggerOpenAIAsync(It.IsAny<TripRequest>()))
            .ReturnsAsync(OpenIaResponseMock.CreateOpenIaInvalidResult());

        var advisorService = new AdvisorService(_loggerMock.Object,
                                                _promptServiceMock.Object,
                                                _notificatorMock.Object);
        //Act
        var result = await advisorService.CreateTripAdviceAsync(tripRequest);

        //Asset
        Assert.Equal(result.StatusCode, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTripAdviceAsync_OpenAIResponseNull_ReturnNotFound()
    {
        //Arrange 
        var tripRequest = new TripRequest(2, "Lisbon", 100, 100);

        _promptServiceMock.Setup(x => x.TriggerOpenAIAsync(It.IsAny<TripRequest>()))
            .ReturnsAsync(OpenIaResponseMock.CreateOpenIaNullContentResult());

        var advisorService = new AdvisorService(_loggerMock.Object,
                                                _promptServiceMock.Object,
                                                _notificatorMock.Object);
        //Act
        var result = await advisorService.CreateTripAdviceAsync(tripRequest);

        //Asset
        Assert.Equal(result.StatusCode, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTripAdviceAsync_OpenAIResponseEmpty_ReturnOk()
    {
        //Arrange 
        var tripRequest = new TripRequest(2, "Lisbon", 100, 100);

        _promptServiceMock.Setup(x => x.TriggerOpenAIAsync(It.IsAny<TripRequest>()))
            .ReturnsAsync(OpenIaResponseMock.CreateOpenIaEmptyContentResult());

        var advisorService = new AdvisorService(_loggerMock.Object,
                                                _promptServiceMock.Object,
                                                _notificatorMock.Object);
        //Act
        var result = await advisorService.CreateTripAdviceAsync(tripRequest);

        //Asset
        Assert.Equal(result.StatusCode, HttpStatusCode.NotFound);
    }
}