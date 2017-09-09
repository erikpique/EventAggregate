using EventAggregate.Core.Interfaces;
using EventAggregate.Core.Test.Models;
using NUnit.Framework;
using System;

namespace EventAggregate.Core.Test
{
    [TestFixture]
    public class EventAggregateTest
    {
        private IEventAggregate _eventAggregate;
        private Customer _customer;
        private Boss _boss;

        [OneTimeSetUp]
        public void Setup()
        {
            _eventAggregate = new Implementation.EventAggregate();
            _customer = new Customer(_eventAggregate);
            _boss = new Boss(_eventAggregate);
        }

        [Test]
        public void Register_NewInstanceIsRegisterWithEventAggregationEqualNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Customer(null));
        }

        [TestCase("aaa")]
        [TestCase("bbb")]
        public void Publish_NotifyEventSetNameOfCustomer_SetNewName(string value)
        {
            _eventAggregate.Publish(value);

            Assert.AreEqual(value, _customer.Name);
        }

        [TestCase(10)]
        [TestCase(20)]
        public void Publish_NotifyEventSetAgeOfCustomer_SetNewAge(int value)
        {
            _eventAggregate.Publish(value);

            Assert.AreEqual(value, _customer.Age);
        }

        [TestCase("aaa")]
        [TestCase("bbb")]
        public void Publish_NotifyEventSetNameToCustomerAndBoss_SetNewName(string value)
        {
            _eventAggregate.Publish(value);

            Assert.AreEqual(value, _customer.Name);
            Assert.AreEqual(value, _boss.Name);
        }

        [TestCase("aaa", 10)]
        [TestCase("bbb", 20)]
        public void Publish_NotifyEventSetNameAndAgeForCustomer_SetNewNameAndAge(string name, int age)
        {
            _eventAggregate.Publish(name);
            _eventAggregate.Publish(age);

            Assert.AreEqual(name, _customer.Name);
            Assert.AreEqual(age, _customer.Age);
        }
    }
}
