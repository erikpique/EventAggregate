using EventAggregate.Core.Interfaces;
using System;

namespace EventAggregate.Core.Test.Models
{
    public class Customer : ISubscriber<string>, ISubscriber<int>
    {
        public string Name { get; private set; }

        public int Age { get; private set; }

        public Customer(IEventAggregate eventAggregate)
        {
            if (eventAggregate == null)
            {
                throw new ArgumentNullException(nameof(eventAggregate));
            }
            eventAggregate.Subscriber(this);
        }

        public void OnNotify(string notify)
        {
            Name = notify;
        }

        public void OnNotify(int notify)
        {
            Age = notify;
        }
    }
}
