namespace EventAggregate.Core.Interfaces
{
    public interface IEventAggregate
    {
        void Subscriber(object subject);

        void Publish<TEvent>(TEvent @event);
    }
}
