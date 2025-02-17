using MassTransit;
using NewProject.Services.RabbitMqEvents;

namespace NewProject.Services.Consumers;

public class SendMailEventConsumer : IConsumer<SendMailEvent>
{
    public async Task Consume(ConsumeContext<SendMailEvent> context)
    {
        var mailEvent = context.Message;

        // Here you would call your email service.
        // For example, using a hypothetical EmailService:
        //await EmailService.SendEmailAsync(mailEvent.Recipient, mailEvent.Subject, mailEvent.Body);

        // You can log the result or handle failures as needed.
    }
}