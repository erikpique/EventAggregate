using EventAggregate.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EventAggregate.Core.Implementation
{
    public sealed class EventAggregate : IEventAggregate
    {
        private readonly IDictionary<Type, List<WeakReference>> _eventAggregates;

        public EventAggregate()
        {
            _eventAggregates = new ConcurrentDictionary<Type, List<WeakReference>>();
        }

        public void Publish<TEvent>(TEvent @event)
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

        public void Subscriber(object subject)
        {
            var interfaceTypes = subject.GetType().GetInterfaces()
                .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ISubscriber<>));

            var weakReference = new WeakReference(subject);

            foreach (var interfaceType in interfaceTypes)
            {
                var subscribers = GetSubscribes(interfaceType);
                subscribers.Add(weakReference);
            }
        }

        private List<WeakReference> GetSubscribes(Type interfaceType)
        {
            List<WeakReference> subscribers;

            if (!_eventAggregates.TryGetValue(interfaceType, out subscribers))
            {
                subscribers = new List<WeakReference>();
                _eventAggregates.Add(interfaceType, subscribers);
            }

            return subscribers;
        }
    }
}
