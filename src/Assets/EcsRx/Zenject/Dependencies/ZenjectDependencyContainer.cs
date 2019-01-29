using System;
using System.Collections;
using System.Linq;
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

        public void Bind(Type fromType, Type toType, BindingConfiguration configuration = null)
        {
            var bindingSetup = _container.Bind(fromType);
            
            if (configuration == null)
            {
                bindingSetup.To(toType).AsSingle();
                return;
            }

            if (configuration.ToInstance != null)
            {
                var instanceBinding = bindingSetup.FromInstance(configuration.ToInstance);
                
                if(configuration.AsSingleton)
                { instanceBinding.AsSingle(); }

                return;
            }
            
            
            if (configuration.ToMethod != null)
            {
                if(configuration.AsSingleton)
                { bindingSetup.AsSingle(); }
                else
                { bindingSetup.AsTransient(); }

                bindingSetup.FromMethodUntyped(x =>  configuration.ToMethod(this));
                return;
            }
            
            var binding = bindingSetup.To(toType);
            
            if(!string.IsNullOrEmpty(configuration.WithName))
            { binding.WithConcreteId(configuration.WithName); }
            
            if(configuration.AsSingleton)
            { binding.AsSingle(); }

            if (configuration.WithNamedConstructorArgs.Count > 0)
            { binding.WithArguments(configuration.WithNamedConstructorArgs.Values); }

            if (configuration.WithTypedConstructorArgs.Count > 0)
            {
                var typePairs = configuration.WithTypedConstructorArgs.Select(x => new TypeValuePair(x.Key, x.Value));
                binding.WithArgumentsExplicit(typePairs);
            }
        }

        public void Bind(Type type, BindingConfiguration configuration = null)
        { Bind(type, type, configuration); }

        public object Resolve(Type type, string name = null)
        {
            if(string.IsNullOrEmpty(name))
            { return _container.Resolve(type); }

            return _container.ResolveId(type, name);
        }

        public bool HasBinding(Type type, string name = null)
        { return _container.HasBindingId(type, name); }

        public void Unbind(Type type)
        { _container.Unbind(type); }

        public IEnumerable ResolveAll(Type type)
        { return _container.ResolveAll(type); }
    }
}