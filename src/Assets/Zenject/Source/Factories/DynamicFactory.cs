using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public interface IDynamicFactory : IValidatable
    {
    }

    // Dynamic factories can be used to choose a creation method in an installer, using FactoryBinder
    public abstract class DynamicFactory<TValue> : IDynamicFactory
    {
        IProvider _provider;
        InjectContext _injectContext;

        [Inject]
        void Init(IProvider provider, InjectContext injectContext)
        {
            Assert.IsNotNull(provider);
            Assert.IsNotNull(injectContext);

            _provider = provider;
            _injectContext = injectContext;
        }

        protected TValue CreateInternal(List<TypeValuePair> extraArgs)
        {
            try
            {
                var result = _provider.GetInstance(_injectContext, extraArgs);

                Assert.That(result == null || result.GetType().DerivesFromOrEqual<TValue>());

                return (TValue)result;
            }
            catch (Exception e)
            {
                throw new ZenjectException(
                    e, "Error during construction of type '{0}' via {1}.Create method!",
                    typeof(TValue).Name(), this.GetType().Name());
            }
        }

        public virtual void Validate()
        {
            try
            {
                _provider.GetInstance(
                    _injectContext, ValidationUtil.CreateDefaultArgs(ParamTypes.ToArray()));
            }
            catch (Exception e)
            {
                throw new ZenjectException(
                    e, "Validation for factory '{0}' failed", this.GetType().Name());
            }
        }

        protected abstract IEnumerable<Type> ParamTypes
        {
            get;
        }
    }
}
