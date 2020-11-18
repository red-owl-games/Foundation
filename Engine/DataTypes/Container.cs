using System;
using System.Collections.Generic;

namespace RedOwl.Engine
{
    public interface IService
    {
        IContainer Container { get; set; }
        void Init();
    }
    
    public interface IContainer : IDisposable
    {
        T AddService<T>(string key = null) where T : IService, new ();
        T AddService<T>(T service, string key = null) where T : IService;
        T GetService<T>(string key = null) where T : IService;
        ICollection<IService> Services();
    }

    public abstract class Service : IService
    {
        public IContainer Container { get; set; }

        public abstract void Init();
    }

    public class Container : IContainer
    {
        private readonly Dictionary<string, IService> services = new Dictionary<string, IService> ();
        private bool isStarted;

        public T AddService<T>(string key = null) where T : IService, new() => AddService(new T (), key);

        public T AddService<T>(T service, string key = null) where T : IService
        {
            Game.BindAs(service);
            services.Add(key ?? typeof(T).Name, service);
            service.Container = this;
            if (isStarted) Start(service);
            return service;
        }

        public T GetService<T>(string key = null) where T : IService => 
            services.TryGetValue(key ?? typeof(T).Name, out var service) ? (T) service : default;

        public ICollection<IService> Services() => services.Values;

        public void Dispose()
        {
            foreach (var service in Services())
            {
                if (service is IDisposable disposable) disposable.Dispose();
            }
        }

        public void Start()
        {
            isStarted = true;
            foreach (var service in Services())
            {
                Start(service);
            }
        }

        private static void Start(IService service)
        {
            Game.Inject(service);
            service.Init();
        }
    }
}