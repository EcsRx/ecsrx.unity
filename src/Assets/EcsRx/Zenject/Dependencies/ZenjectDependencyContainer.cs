using System.Collections.Generic;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Unity.Dependencies;
using UnityEngine;
using Zenject;

namespace EcsRx.Zenject.Dependencies
{
    public class ZenjectDependencyContainer : IDependencyContainer, IUnityInstantiator
    {
        private readonly DiContainer _container;

        public ZenjectDependencyContainer(DiContainer container)
        {
            _container = container;
            _container.Bind<IUnityInstantiator>().FromInstance(this);
            _container.Bind<IDependencyContainer>().FromInstance(this);
        }

        public void LoadModule(IDependencyModule module)
        {
            module.Setup(this);
        }

        public object NativeContainer => _container;
        
        public GameObject InstantiatePrefab(GameObject prefab)
        { return _container.InstantiatePrefab(prefab); }

        public void Bind<TFrom, TTo>(BindingConfiguration configuration = null) where TTo : TFrom
        {
            var bindingSetup = _container.Bind<TFrom>();
            
            if (configuration == null)
            {
                bindingSetup.To<TTo>().AsSingle();
                return;
            }

            if (configuration.BindInstance != null)
            {
                var instanceBinding = bindingSetup.FromInstance((TFrom)configuration.BindInstance);
                
                if(configuration.AsSingleton)
                { instanceBinding.AsSingle(); }

                return;
            }

            if(!string.IsNullOrEmpty(configuration.WithName))
            { bindingSetup.WithId(configuration.WithName); }
            
            var binding = bindingSetup.To<TFrom>();
            
            if(configuration.AsSingleton)
            { binding.AsSingle(); } 
            
            if (configuration.WithConstructorArgs.Count == 0)
            { return; }

            binding.WithArguments(configuration.WithConstructorArgs.Values);
            _container.Bind<TFrom>().To<TTo>().AsSingle();
        }

        public void Bind<T>(BindingConfiguration configuration = null)
        { Bind<T,T>(configuration); }

        public T Resolve<T>(string name = null)
        {
            if(string.IsNullOrEmpty(name))
            { return _container.Resolve<T>(); }

            return _container.ResolveId<T>(name);
        }

        public void Unbind<T>()
        {
            _container.Unbind<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        { return _container.ResolveAll<T>(); }

        public void LoadModule<T>() where T : IDependencyModule, new()
        {
            var module = new T();
            LoadModule(module);
        }
    }
}