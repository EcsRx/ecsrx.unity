using ModestTree;

namespace Zenject
{
    public class BindFinalizerWrapper : IBindingFinalizer
    {
        readonly string _missingFinalizerMessage;

        public BindFinalizerWrapper(string missingFinalizerMessage)
        {
            _missingFinalizerMessage = missingFinalizerMessage;
        }

        public IBindingFinalizer SubFinalizer
        {
            get; set;
        }

        public BindingInheritanceMethods BindingInheritanceMethod
        {
            get
            {
                AssertHasFinalizer();
                return SubFinalizer.BindingInheritanceMethod;
            }
        }

        void AssertHasFinalizer()
        {
            if (SubFinalizer == null)
            {
                throw Assert.CreateException(
                    "Unfinished binding!  Some required information was left unspecified.  {0}",
                    _missingFinalizerMessage == null ? "" : _missingFinalizerMessage);
            }
        }

        public void FinalizeBinding(DiContainer container)
        {
            AssertHasFinalizer();
            SubFinalizer.FinalizeBinding(container);
        }
    }
}
