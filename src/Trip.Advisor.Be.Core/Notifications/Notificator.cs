using Microsoft.Extensions.Logging;

namespace  Trip.Advisor.Be.Core.Notifications
{
    public class Notificator : INotificator
    {
        private readonly List<Notification> _notifications;
        private readonly ILogger<Notification> _logger;

        public Notificator(ILogger<Notification> logger)
        {
            _notifications = new List<Notification>();
            _logger = logger;
        }

        public List<Notification> GetNotifications() => _notifications;

        public void Handle(Notification notificacao)
        {
            _notifications.Add(notificacao);
            _logger.LogError(notificacao.Message);
        }
        public void Handle(LogLevel logLevel, Notification notificacao)
        {
            _notifications.Add(notificacao);
            _logger.LogError($"{logLevel} - {notificacao.Message}");
        }

        public bool HasNotification() => _notifications.Any();

        public void CleanNotifications() => _notifications.Clear();
    }
}
