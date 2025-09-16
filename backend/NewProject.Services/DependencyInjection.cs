using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NewProject.Services.Consumers;
using NewProject.Services.Interface;
using NewProject.Services.Services;

namespace NewProject.Services;

public static class DependencyInjection
{
    public static void AddRegisterServicesDependency(this IServiceCollection services)
    {
        services.AddScoped<IXApiServices, XApiServices>();
        services.AddScoped<IFacebookApiService, FacebookApiService>();
    }

    public static void RegisterRabbitMqConsumer(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<SendMailEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("sendmail-event-queue", e =>
                {
                    e.ConfigureConsumer<SendMailEventConsumer>(context);
                });
            });
        });
    }
}