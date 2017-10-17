using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Core.Dependencies
{
    public static class DependencyContainer
    {
        private static ConcurrentDictionary<Type, object> Instances = new ConcurrentDictionary<Type, object>();

        public static void Register<T>(object instance)
        {
            if (Instances.TryAdd(typeof(T), instance) == false)
                throw new Exception($"Could not add instance of {typeof(T).FullName} to InstanceContainer");
        }

        public static T Resolve<T>() where T : class
        {
            if (Instances.ContainsKey(typeof(T)) == false)
                return null as T;

            return Instances[typeof(T)] as T;
        } 
    }
}
