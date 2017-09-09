using EventAggregate.Core.Implementation.Base;
using EventAggregate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EventAggregate.Core.Implementation
{
    public sealed class EventAggregate : EventAggregateBase
    {
        public override void Publish<TEvent>(TEvent @event)
        {
            var subscribeType = typeof(ISubscriber<>).MakeGenericType(typeof(TEvent));
            var subscribersToRemove = new List<WeakReference>();
            var subscribers = GetSubscribes(subscribeType);

            foreach (var subscriber in subscribers)
            {
                if (subscriber.IsAlive)
                {
                    var subscriberEvent = (ISubscriber<TEvent>)subscriber.Target;
                    var syncContext = SynchronizationContext.Current ?? new SynchronizationContext();
                    syncContext.Send(context => subscriberEvent.OnNotify(@event), null);
                }
                else
                {
                    subscribersToRemove.Add(subscriber);
                }
            }
            if (subscribersToRemove.Any())
            {
                subscribersToRemove.ForEach(subscriber => subscribers.Remove(subscriber));
            }
        }
    }
}
