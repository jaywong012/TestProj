namespace Domain.Interfaces;

public interface IIntegrationEventBus
{
    Task PublishAsync<T>(T @event) where T : class;
}