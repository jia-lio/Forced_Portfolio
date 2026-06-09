using System;
using System.Collections.Generic;

namespace Nullbytes
{
    public class ManagerContainer
    {
        private readonly Dictionary<Type, ISubManager> managers = new();
        
        public GameConfig Config { get; private set; }

        public ManagerContainer(GameConfig config)
        {
            this.Config = config;
        }
        
        public void CleanUp()
        {
            //managers.Clear();
            //Config = null;
        }

        public void Register<T>(T instance) where T : class, ISubManager
        {
            if (managers.ContainsKey(typeof(T)))
                throw new InvalidOperationException($"Manager of type {typeof(T)} already registered.");
            managers[typeof(T)] = instance;
        }
        
        public T Resolve<T>() where T : class, ISubManager
        {
            if (!managers.TryGetValue(typeof(T), out var instance))
                throw new InvalidOperationException($"Manager of type {typeof(T)} not registered.");
            return instance as T;
        }
        
        public object Resolve(Type type)
        {
            if (!managers.TryGetValue(type, out var instance))
                throw new InvalidOperationException($"Manager of type {type} not registered.");
            return instance;
        }
        
        public bool IsRegistered<T>() where T : class, ISubManager
        {
            return managers.ContainsKey(typeof(T));
        }

        public IEnumerable<ISubManager> GetAll() => managers.Values;
    }
}