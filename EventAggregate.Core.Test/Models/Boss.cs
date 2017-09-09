using EventAggregate.Core.Interfaces;
using System;

namespace EventAggregate.Core.Test.Models
{
    public class Boss : ISubscriber<string>
    {
        public string Name { get; private set; }

        public Boss(IEventAggregate eventAggregate)
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
    }
}
