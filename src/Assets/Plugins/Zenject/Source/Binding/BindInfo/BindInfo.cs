using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Zenject
{
    public enum ScopeTypes
    {
        Unset,
        Transient,
        Singleton,
    }

    public enum ToChoices
    {
        Self,
        Concrete,
    }

    public enum InvalidBindResponses
    {
        Assert,
        Skip,
    }

    public enum BindingInheritanceMethods
    {
        None,
        CopyIntoAll,
        CopyDirectOnly,
        MoveIntoAll,
        MoveDirectOnly,
    }

    public class BindInfo
    {
        public BindInfo()
        {
            ContextInfo = null;
            Identifier = null;
            ConcreteIdentifier = null;
            ContractTypes = new List<Type>();
            ToTypes = new List<Type>();
            Arguments = new List<TypeValuePair>();
            ToChoice = ToChoices.Self;
            BindingInheritanceMethod = BindingInheritanceMethods.None;
            OnlyBindIfNotBound = false;
            SaveProvider = false;

            // Change this to true if you want all dependencies to be created at the start
            NonLazy = false;

            MarkAsUniqueSingleton = false;
            MarkAsCreationBinding = true;

            Scope = ScopeTypes.Unset;
            InvalidBindResponse = InvalidBindResponses.Assert;
        }

        [Conditional("UNITY_EDITOR")]
        public void SetContextInfo(string contextInfo)
        {
            ContextInfo = contextInfo;
        }

        public bool MarkAsCreationBinding;

        public bool MarkAsUniqueSingleton;

        public object ConcreteIdentifier;

        public bool SaveProvider;

        public bool OnlyBindIfNotBound;

        public bool RequireExplicitScope;

        public object Identifier;

        public List<Type> ContractTypes;

        public BindingInheritanceMethods BindingInheritanceMethod;

        public InvalidBindResponses InvalidBindResponse;

        public bool NonLazy;

        public BindingCondition Condition;

        public ToChoices ToChoice;

        public string ContextInfo
        {
            get;
            private set;
        }

        // Only relevant with ToChoices.Concrete
        public List<Type> ToTypes;

        public ScopeTypes Scope;

        public List<TypeValuePair> Arguments;
    }
}
