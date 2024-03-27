using System;
using SystemsRx.Infrastructure.Dependencies;
using EcsRx.Unity.Dependencies;
using Zenject;

namespace EcsRx.Zenject.Dependencies
{
    public class ZenjectDependencyRegistry : IDependencyRegistry
    {
        private readonly DiContainer _container;
        public object NativeRegistry => _container;
        public ZenjectDependencyResolver Resolver { get; }

        public ZenjectDependencyRegistry(DiContainer container)
        {
            _container = container;
            Resolver = new ZenjectDependencyResolver(container);
            
            _container.Bind<IDependencyRegistry>().FromInstance(this);
            _container.Bind<IDependencyResolver>().FromInstance(Resolver);
            _container.Bind<IUnityInstantiator>().FromInstance(Resolver);
        }

        public void LoadModule(IDependencyModule module)
        {
            module.Setup(this);
        }

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
            { binding = bindingSetup.FromMethodUntyped(x =>  configuration.ToMethod(Resolver)); }
            else
            { binding = bindingSetup.To(toType); }

            if (configuration.AsSingleton)
            { binding.AsSingle(); }
            else
            { binding.AsTransient();}
        }

        public void Bind(Type type, BindingConfiguration configuration = null)
        { Bind(type, type, configuration); }

        public bool HasBinding(Type type, string name = null)
        { return _container.HasBindingId(type, name); }

        public void Unbind(Type type)
        { _container.Unbind(type); }

        public IDependencyResolver BuildResolver()
        { return Resolver; }

        public void Dispose()
        {}
    }
}