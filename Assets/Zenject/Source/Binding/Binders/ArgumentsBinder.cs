using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
    public class ArgumentsBinder : ConditionBinder
    {
        public ArgumentsBinder(BindInfo bindInfo)
            : base(bindInfo)
        {
        }

        public ConditionBinder WithArguments(params object[] args)
        {
            BindInfo.Arguments = InjectUtil.CreateArgList(args);
            return this;
        }

        public ConditionBinder WithArgumentsExplicit(IEnumerable<TypeValuePair> extraArgs)
        {
            BindInfo.Arguments = extraArgs.ToList();
            return this;
        }
    }
}
