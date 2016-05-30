using System;
using System.Collections.Generic;
using ModestTree;

#if !NOT_UNITY3D
using UnityEngine;
#endif


namespace Zenject
{
    public class FactoryToChoiceBinder<TParam1, TContract> : FactoryFromBinder<TParam1, TContract>
    {
        public FactoryToChoiceBinder(
            BindInfo bindInfo, Type factoryType, 
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, factoryType, finalizerWrapper)
        {
        }

        // Note that this is the default, so not necessary to call
        public FactoryFromBinder<TParam1, TContract> ToSelf()
        {
            Assert.IsEqual(BindInfo.ToChoice, ToChoices.Self);
            return this;
        }

        public FactoryFromBinder<TParam1, TConcrete> To<TConcrete>()
            where TConcrete : TContract
        {
            BindInfo.ToChoice = ToChoices.Concrete;
            BindInfo.ToTypes = new List<Type>()
            {
                typeof(TConcrete)
            };

            return new FactoryFromBinder<TParam1, TConcrete>(
                BindInfo, FactoryType, FinalizerWrapper);
        }
    }

    public class FactoryToChoiceIdBinder<TParam1, TContract> : FactoryToChoiceBinder<TParam1, TContract>
    {
        public FactoryToChoiceIdBinder(
            BindInfo bindInfo, Type factoryType, 
            BindFinalizerWrapper finalizerWrapper)
            : base(bindInfo, factoryType, finalizerWrapper)
        {
        }

        public FactoryToChoiceBinder<TParam1, TContract> WithId(object identifier)
        {
            BindInfo.Identifier = identifier;
            return this;
        }
    }
}
