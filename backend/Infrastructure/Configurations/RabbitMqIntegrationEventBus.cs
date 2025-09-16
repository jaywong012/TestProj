using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Configurations;

public class RabbitMqIntegrationEventBus : IIntegrationEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public RabbitMqIntegrationEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T @event) where T : class
    {
        return _publishEndpoint.Publish(@event);
    }
}