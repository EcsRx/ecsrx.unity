using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class ConditionBinder : InheritInSubContainersBinder
    {
        public ConditionBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public InheritInSubContainersBinder When(BindingCondition condition)
        {
            BindInfo.Condition = condition;
            return this;
        }

        public InheritInSubContainersBinder WhenInjectedIntoInstance(object instance)
        {
            BindInfo.Condition = r => ReferenceEquals(r.ObjectInstance, instance);
            return this;
        }

        public InheritInSubContainersBinder WhenInjectedInto(params Type[] targets)
        {
            BindInfo.Condition = r => targets.Where(x => r.ObjectType != null && r.ObjectType.DerivesFromOrEqual(x)).Any();
            return this;
        }

        public InheritInSubContainersBinder WhenInjectedInto<T>()
        {
            BindInfo.Condition = r => r.ObjectType != null && r.ObjectType.DerivesFromOrEqual(typeof(T));
            return this;
        }

        public InheritInSubContainersBinder WhenNotInjectedInto<T>()
        {
            BindInfo.Condition = r => r.ObjectType == null || !r.ObjectType.DerivesFromOrEqual(typeof(T));
            return this;
        }
    }
}
