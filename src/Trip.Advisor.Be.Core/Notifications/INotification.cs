using Microsoft.Extensions.Logging;

namespace  Trip.Advisor.Be.Core.Notifications;

public interface INotificator
{
    bool HasNotification();
    List<Notification> GetNotifications();
    void Handle(Notification notification);
    void Handle(LogLevel logLevel, Notification notification);
    void CleanNotifications();
}
