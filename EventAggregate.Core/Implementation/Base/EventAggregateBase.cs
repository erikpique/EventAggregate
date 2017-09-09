using EventAggregate.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace EventAggregate.Core.Implementation.Base
{
    public abstract class EventAggregateBase : IEventAggregate
    {
        private readonly IDictionary<Type, List<WeakReference>> _eventAggregates;

        protected EventAggregateBase()
        {
            _eventAggregates = new ConcurrentDictionary<Type, List<WeakReference>>();
        }

        public abstract void Publish<TEvent>(TEvent @event);

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

        protected List<WeakReference> GetSubscribes(Type interfaceType)
        {
            if (!_eventAggregates.TryGetValue(interfaceType, out List<WeakReference> subscribers))
            {
                subscribers = new List<WeakReference>();
                _eventAggregates.Add(interfaceType, subscribers);
            }

            return subscribers;
        }
    }
}
