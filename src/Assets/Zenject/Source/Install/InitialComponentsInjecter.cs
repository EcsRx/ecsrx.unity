#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using System.Linq;
using ModestTree.Util;

namespace Zenject
{
    public class InitialComponentsInjecter : IProvider
    {
        readonly DiContainer _container;
        readonly Dictionary<Component, ComponentInfo> _componentMap = new Dictionary<Component, ComponentInfo>();
        readonly Dictionary<BindingId, List<ComponentInfo>> _bindings = new Dictionary<BindingId, List<ComponentInfo>>();

        public InitialComponentsInjecter(
            DiContainer container, List<Component> injectableComponents)
        {
            _container = container;
            _componentMap = injectableComponents.ToDictionary(x => x, x => new ComponentInfo(x));

            // Installers are always injected just before calling Install()
            Assert.That(injectableComponents.Where(x => x.GetType().DerivesFrom<MonoInstaller>()).IsEmpty());
        }

        public IEnumerable<Component> Components
        {
            get
            {
                return _componentMap.Keys;
            }
        }

        public void InstallBinding(ZenjectBinding binding)
        {
            if (!binding.enabled)
            {
                return;
            }

            if (binding.Components == null || binding.Components.IsEmpty())
            {
                Log.Warn("Found empty list of components on ZenjectBinding on object '{0}'", binding.name);
                return;
            }

            string identifier = null;

            if (binding.Identifier.Trim().Length > 0)
            {
                identifier = binding.Identifier;
            }

            foreach (var component in binding.Components)
            {
                var bindType = binding.BindType;

                if (component == null)
                {
                    Log.Warn("Found null component in ZenjectBinding on object '{0}'", binding.name);
                    continue;
                }

                var componentType = component.GetType();

                ComponentInfo componentInfo;

                if (_componentMap.TryGetValue(component, out componentInfo))
                {
                    switch (bindType)
                    {
                        case ZenjectBinding.BindTypes.Self:
                        {
                            InstallSingleBinding(componentType, identifier, componentInfo);
                            break;
                        }
                        case ZenjectBinding.BindTypes.AllInterfaces:
                        {
                            foreach (var baseType in componentType.Interfaces())
                            {
                                InstallSingleBinding(baseType, identifier, componentInfo);
                            }

                            break;
                        }
                        case ZenjectBinding.BindTypes.AllInterfacesAndSelf:
                        {
                            foreach (var baseType in componentType.Interfaces())
                            {
                                InstallSingleBinding(baseType, identifier, componentInfo);
                            }
                            InstallSingleBinding(componentType, identifier, componentInfo);

                            break;
                        }
                        default:
                        {
                            throw Assert.CreateException();
                        }
                    }
                }
                else
                {
                    // In this case, we are adding a binding for a component that does not exist
                    // in our 'context'.  So we are not responsible for injecting it - there is
                    // another InitialComponentsInjecter that does this.
                    // Best we can do here is just add the instance to our container
                    // This may result in the instance being injected somewhere without itself
                    // being injected, but there's not much we can do about that
                    InstallNonInjectedBinding(bindType, identifier, component);
                }
            }
        }

        void InstallNonInjectedBinding(ZenjectBinding.BindTypes bindType, string identifier, Component component)
        {
            switch (bindType)
            {
                case ZenjectBinding.BindTypes.Self:
                {
                    _container.Bind(component.GetType()).WithId(identifier).FromInstance(component);
                    break;
                }
                case ZenjectBinding.BindTypes.AllInterfaces:
                {
                    _container.BindAllInterfaces(component.GetType()).WithId(identifier).FromInstance(component);
                    break;
                }
                case ZenjectBinding.BindTypes.AllInterfacesAndSelf:
                {
                    _container.BindAllInterfacesAndSelf(component.GetType()).WithId(identifier).FromInstance(component);
                    break;
                }
                default:
                {
                    throw Assert.CreateException();
                }
            }
        }

        public void LazyInjectComponents()
        {
            foreach (var info in _componentMap.Values)
            {
                LazyInject(info);
            }
        }

        void InstallSingleBinding(Type type, string identifier, ComponentInfo componentInfo)
        {
            var bindingId = new BindingId(type, identifier);

            List<ComponentInfo> infoList;

            if (!_bindings.TryGetValue(bindingId, out infoList))
            {
                infoList = new List<ComponentInfo>();

                _bindings.Add(bindingId, infoList);

                // Note: We only want to register for each unique BindingId once
                // since we return multiple matches in GetAllInstancesWithInjectSplit
                _container.RegisterProvider(bindingId, null, this);
            }

            infoList.Add(componentInfo);
        }

        public Type GetInstanceType(InjectContext context)
        {
            return context.MemberType;
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(
            InjectContext context, List<TypeValuePair> args)
        {
            var infoList = GetBoundInfosWithId(context.GetBindingId());

            yield return infoList.Select(x => (object)x.Component).ToList();

            foreach (var info in infoList)
            {
                LazyInject(info);
            }
        }

        void LazyInject(ComponentInfo info)
        {
            if (!info.HasInjected)
            {
                info.HasInjected = true;
                _container.Inject(info.Component);
            }
        }

        List<ComponentInfo> GetBoundInfosWithId(BindingId bindingId)
        {
            List<ComponentInfo> result;

            if (!_bindings.TryGetValue(bindingId, out result))
            {
                result = new List<ComponentInfo>();
            }

            return result;
        }

        class ComponentInfo
        {
            public ComponentInfo(Component component)
            {
                Component = component;
            }

            public bool HasInjected
            {
                get;
                set;
            }

            public Component Component
            {
                get;
                private set;
            }
        }
    }
}

#endif