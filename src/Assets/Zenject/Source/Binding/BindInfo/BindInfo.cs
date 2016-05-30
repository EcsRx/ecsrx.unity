using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public enum ScopeTypes
    {
        Transient,
        Singleton,
        Cached,
    }

    public enum ToChoices
    {
        Self,
        Concrete,
    }

    public class BindInfo
    {
        public BindInfo(List<Type> contractTypes)
        {
            Identifier = null;
            ContractTypes = contractTypes;
            ToTypes = new List<Type>();
            Arguments = new List<TypeValuePair>();
            ToChoice = ToChoices.Self;
            InheritInSubContainers = false;
            NonLazy = false;
            Scope = ScopeTypes.Transient;
        }

        public BindInfo(Type contractType)
            : this(new List<Type>() { contractType } )
        {
        }

        public BindInfo()
            : this(new List<Type>())
        {
        }

        public object Identifier
        {
            get;
            set;
        }

        public List<Type> ContractTypes
        {
            get;
            set;
        }

        public bool InheritInSubContainers
        {
            get;
            set;
        }

        public bool NonLazy
        {
            get;
            set;
        }

        public BindingCondition Condition
        {
            get;
            set;
        }

        public ToChoices ToChoice
        {
            get;
            set;
        }

        // Only relevant with ToChoices.Concrete
        public List<Type> ToTypes
        {
            get;
            set;
        }

        public ScopeTypes Scope
        {
            get;
            set;
        }

        // Note: This only makes sense for ScopeTypes.Singleton
        public string ConcreteIdentifier
        {
            get;
            set;
        }

        public List<TypeValuePair> Arguments
        {
            get;
            set;
        }
    }
}
