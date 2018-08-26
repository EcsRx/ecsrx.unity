using System;
using System.Collections.Generic;
using ModestTree;

namespace Zenject
{
    public class DecoratorToChoiceFromBinder<TContract>
    {
        DiContainer _bindContainer;
        BindInfo _bindInfo;
        FactoryBindInfo _factoryBindInfo;

        public DecoratorToChoiceFromBinder(
            DiContainer bindContainer, BindInfo bindInfo, FactoryBindInfo factoryBindInfo)
        {
            _bindContainer = bindContainer;
            _bindInfo = bindInfo;
            _factoryBindInfo = factoryBindInfo;
        }

        public FactoryFromBinder<TContract, TConcrete> With<TConcrete>()
            where TConcrete : TContract
        {
            _bindInfo.ToChoice = ToChoices.Concrete;
            _bindInfo.ToTypes = new List<Type>()
            {
                typeof(TConcrete)
            };

            return new FactoryFromBinder<TContract, TConcrete>(
                _bindContainer, _bindInfo, _factoryBindInfo);
        }
    }
}
