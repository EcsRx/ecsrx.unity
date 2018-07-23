using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

namespace Zenject
{
    public class ConcreteBinderGeneric<TContract> : FromBinderGeneric<TContract>
    {
        public ConcreteBinderGeneric(
            DiContainer bindContainer, BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindContainer, bindInfo, finalizerWrapper)
        {
            ToSelf();
        }

        // Note that this is the default, so not necessary to call
        public FromBinderGeneric<TContract> ToSelf()
        {
            Assert.IsEqual(BindInfo.ToChoice, ToChoices.Self);

            BindInfo.RequireExplicitScope = true;
            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo, (container, type) => new TransientProvider(
                    type, container, BindInfo.Arguments,
                    BindInfo.ContextInfo, BindInfo.ConcreteIdentifier));

            return this;
        }

        public FromBinderGeneric<TConcrete> To<TConcrete>()
            where TConcrete : TContract
        {
            BindInfo.ToChoice = ToChoices.Concrete;
            BindInfo.ToTypes = new List<Type>()
            {
                typeof(TConcrete)
            };

            return new FromBinderGeneric<TConcrete>(
                BindContainer, BindInfo, FinalizerWrapper);
        }

        public FromBinderNonGeneric To(params Type[] concreteTypes)
        {
            return To((IEnumerable<Type>)concreteTypes);
        }

        public FromBinderNonGeneric To(IEnumerable<Type> concreteTypes)
        {
            BindingUtil.AssertIsDerivedFromTypes(
                concreteTypes, BindInfo.ContractTypes, BindInfo.InvalidBindResponse);

            BindInfo.ToChoice = ToChoices.Concrete;
            BindInfo.ToTypes = concreteTypes.ToList();

            return new FromBinderNonGeneric(
                BindContainer, BindInfo, FinalizerWrapper);
        }

#if !(UNITY_WSA && ENABLE_DOTNET)
        public FromBinderNonGeneric To(
            Action<ConventionSelectTypesBinder> generator)
        {
            var bindInfo = new ConventionBindInfo();

            // Automatically filter by the given contract types
            bindInfo.AddTypeFilter(
                concreteType => BindInfo.ContractTypes.All(contractType => concreteType.DerivesFromOrEqual(contractType)));

            generator(new ConventionSelectTypesBinder(bindInfo));
            return To(bindInfo.ResolveTypes());
        }
#endif
    }
}
