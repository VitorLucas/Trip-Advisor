using Trip.Advisor.Be.Core.Notifications;
using System.Net;

namespace  Trip.Advisor.Be.Core.Helpers;

public class Result
{
    public Result() { }

    public Result(object data) => Data = data;

    public Result(object data, HttpStatusCode statusCode)
    {
        Data = data;
        StatusCode = statusCode;
    }

    public Result(object data, HttpStatusCode statusCode, bool isSuccess)
    {
        Data = data;
        StatusCode = statusCode;
        IsSuccess = isSuccess;
    }

    public IEnumerable<Notification> Errors { get; set; }
    public object Data { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool IsSuccess { get; set; } = true;
}
