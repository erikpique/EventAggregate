namespace EventAggregate.Core.Interfaces
{
    public interface ISubscriber<TEvent>
    {
        void OnNotify(TEvent @event);
    }
}
