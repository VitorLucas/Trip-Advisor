using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Trip.Advisor.Be.Core.Notifications;
using System.Text;
using System.Text.Json;

namespace  Trip.Advisor.Be.Application.Services;

public class ServiceBase
{
    private readonly INotificator _notificador;
    protected ServiceBase(INotificator notificador) => _notificador = notificador;

    protected void Notify(ValidationResult validationResult)
    {
        foreach (ValidationFailure? error in validationResult.Errors)
        {
            Notify(error.ErrorMessage);
        }
    }

    protected void Notify(string mensagem) => _notificador.Handle(new Notification(mensagem));
    protected void Notify(LogLevel logLevel, string mensagem) => _notificador.Handle(logLevel, new Notification(mensagem));

    protected static StringContent GetContent(object dado) => new StringContent(JsonSerializer.Serialize(dado), Encoding.UTF8, "application/json");

    protected static async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error on Deserialize, JSON:{await responseMessage.Content.ReadAsStringAsync()};", ex);
        }
    }

    public static string SerializeObject<T>(T obj)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            WriteIndented = true
        };

        return JsonSerializer.Serialize(obj, options);
    }

    protected bool HasNotifications() => _notificador.HasNotification();

    protected List<string> GetNotifications() => _notificador.GetNotifications().Select(s => s.Message).ToList();
    protected void CleanNotifications() => _notificador.CleanNotifications();
}