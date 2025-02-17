namespace NewProject.Services.RabbitMqEvents;

public class SendMailEvent
{
    public string Recipient { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}