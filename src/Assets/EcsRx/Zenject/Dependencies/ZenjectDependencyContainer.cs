using System;
using System.Collections;
using System.Linq;
using SystemsRx.Infrastructure.Dependencies;
using EcsRx.Unity.Dependencies;
using EcsRx.Zenject.Extensions;
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
            
            if(!string.IsNullOrEmpty(configuration.WithName))
            { bindingSetup.WithId(configuration.WithName); }

            ScopeConcreteIdArgConditionCopyNonLazyBinder binding;
            
            if (configuration.ToInstance != null)
            { binding = bindingSetup.FromInstance(configuration.ToInstance); }
            else if (configuration.ToMethod != null)
            { binding = bindingSetup.FromMethodUntyped(x =>  configuration.ToMethod(this)); }
            else
            {
                binding = bindingSetup.To(toType);

                if (configuration.WithNamedConstructorArgs.Count > 0)
                { binding.WithArguments(configuration.WithNamedConstructorArgs.Values); }

                if (configuration.WithTypedConstructorArgs.Count > 0)
                {
                    var typePairs = configuration.WithTypedConstructorArgs.Select(x => new TypeValuePair(x.Key, x.Value));
                    binding.WithArgumentsExplicit(typePairs);
                }
            }

            if (configuration.AsSingleton)
            { binding.AsSingle(); }
            else
            { binding.AsTransient();}
            
            if(configuration.OnActivation != null)
            { binding.OnInstantiated((context, instance) => { configuration.OnActivation(this, instance); }); }

            if (configuration.WhenInjectedInto != null && configuration.WhenInjectedInto.Count > 0)
            { binding.WhenInjectedInto(configuration.WhenInjectedInto.ToArray()); }
        }

        public void Bind(Type type, BindingConfiguration configuration = null)
        { Bind(type, type, configuration); }

        public object Resolve(Type type, string name = null)
        {
            return string.IsNullOrEmpty(name) ? 
                _container.Resolve(type) : 
                _container.ResolveId(type, name);
        }

        public bool HasBinding(Type type, string name = null)
        { return _container.HasBindingId(type, name); }

        public void Unbind(Type type)
        { _container.Unbind(type); }

        public IEnumerable ResolveAll(Type type)
        { return _container.ResolveAllOf(type); }

        public void Dispose()
        {}
    }
}