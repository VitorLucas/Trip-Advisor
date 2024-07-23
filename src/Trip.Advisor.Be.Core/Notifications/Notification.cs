namespace  Trip.Advisor.Be.Core.Notifications;

public class Notification
{
    public Notification(string mensagem) => Message = mensagem;

    public Notification(string mensagem, string field)
    {
        Message = mensagem;
        Field = field;
    }

    public string Field { get; }
    public string Message { get; }
}
