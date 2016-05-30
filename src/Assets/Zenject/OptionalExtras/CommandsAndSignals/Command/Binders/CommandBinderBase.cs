using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ModestTree;
using ModestTree.Util;
using System.Linq;

namespace Zenject
{
    public class CommandBinderBase<TCommand, TAction>
        where TCommand : ICommand
        where TAction : class
    {
        readonly DiContainer _container;
        readonly BindFinalizerWrapper _finalizerWrapper;
        readonly BindInfo _bindInfo;

        public CommandBinderBase(string identifier, DiContainer container)
        {
            _container = container;

            _bindInfo = new BindInfo();
            _bindInfo.Identifier = identifier;
            _bindInfo.ContractTypes = new List<Type>()
                {
                    typeof(TCommand),
                };

            _finalizerWrapper = container.StartBinding();
        }

        protected BindInfo BindInfo
        {
            get
            {
                return _bindInfo;
            }
        }

        protected IBindingFinalizer Finalizer
        {
            set
            {
                _finalizerWrapper.SubFinalizer = value;
            }
        }

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }
    }
}

