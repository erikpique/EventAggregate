using AbstractFactory.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AbstractFactory.Core.Implementation
{
    public class AbstractFactory : IAbstractFactory
    {
        private readonly IDictionary<Type, Type[]> _instances;

        public AbstractFactory()
        {
            _instances = new ConcurrentDictionary<Type, Type[]>();
        }

        public TOutput CreateInstance<TOutput>(params object[] parameters)
        {
            try
            {
                Type[] instances;
                if (!_instances.TryGetValue(typeof(TOutput), out instances))
                {
                    instances = GetTypeAssembly<TOutput>();
                }

                return (TOutput)Activator.CreateInstance(instances.First(), parameters);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private Type[] GetTypeAssembly<TType>()
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.GetGenericTypeDefinition() == typeof(TType))
                .ToArray();

            _instances.Add(typeof(TType), types);

            return types;
        }

    }
}
