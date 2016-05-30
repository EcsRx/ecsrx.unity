using System;
using System.Collections.Generic;
using ModestTree;
using System.Linq;

#if !NOT_UNITY3D
using UnityEngine;
#endif

namespace Zenject
{
    public class ConcreteBinderGeneric<TContract> : FromBinderGeneric<TContract>
    {
        public ConcreteBinderGeneric(
            BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, finalizerWrapper)
        {
            ToSelf();
        }

        // Note that this is the default, so not necessary to call
        public FromBinderGeneric<TContract> ToSelf()
        {
            Assert.IsEqual(BindInfo.ToChoice, ToChoices.Self);

            SubFinalizer = new ScopableBindingFinalizer(
                BindInfo, SingletonTypes.To, null,
                (container, type) => new TransientProvider(
                    type, container, BindInfo.Arguments, BindInfo.ConcreteIdentifier));

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
                BindInfo, FinalizerWrapper);
        }

        public FromBinderNonGeneric To(params Type[] concreteTypes)
        {
            return To((IEnumerable<Type>)concreteTypes);
        }

        public FromBinderNonGeneric To(IEnumerable<Type> concreteTypes)
        {
            BindingUtil.AssertIsDerivedFromTypes(concreteTypes, BindInfo.ContractTypes);

            BindInfo.ToChoice = ToChoices.Concrete;
            BindInfo.ToTypes = concreteTypes.ToList();

            return new FromBinderNonGeneric(
                BindInfo, FinalizerWrapper);
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
