using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Trip.Advisor.Be.Core.Notifications;
using Trip.Advisor.Be.Core.Helpers;

namespace  Trip.Advisor.Be.Api.V1;

[ApiController]
public class MainController : ControllerBase
{
    private readonly INotificator _notificator;

    protected MainController(INotificator notificator) => _notificator = notificator;

    protected bool ValidOperation() => !_notificator.HasNotification();

    protected ActionResult CustomResponse(object result = null) => ValidOperation()
            ? Ok(new
            {
                success = true,
                data = result
            })
            : BadRequest(new
            {
                success = false,
                errors = _notificator.GetNotifications().Select(n => n.Message)
            });

    protected ActionResult CustomResponse<TResult>(Result result)
    {
        ActionResult actionResult;

        if (ValidOperation())
        {
            result.IsSuccess = true;
            actionResult = result.StatusCode switch
            {
                HttpStatusCode.Created => Created($"{Request.Path.Value}{result.Data}", result),
                HttpStatusCode.NoContent => NoContent(),
                _ => Ok(result)
            };
        }
        else
        {
            result.IsSuccess = false;
            result.Errors = _notificator.GetNotifications().ToArray();

            actionResult = result.StatusCode switch
            {
                HttpStatusCode.NotFound => NotFound(result),
                HttpStatusCode.Unauthorized => Unauthorized(result),
                HttpStatusCode.Conflict => Conflict(result),
                _ => BadRequest(result)
            };
        }

        return actionResult;
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
        {
            NotifyError(modelState);
        }

        return CustomResponse();
    }

    protected void NotifyError(string mensagem, string field) => _notificator.Handle(new Notification(mensagem, field));

    protected void NotifyError(string mensagem) => _notificator.Handle(new Notification(mensagem));

    protected void NotifyError(ModelStateDictionary modelState)
    {
        if (modelState?.Values is null)
        {
            _notificator.Handle(new Notification("Model state is null."));
            return;
        }

        var errors = ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
            );

        var notifications = new List<Notification>();

        foreach (KeyValuePair<string, string?> error in errors)
        {
            _notificator.Handle(new Notification(error.Value, error.Key));
        }
    }
}
