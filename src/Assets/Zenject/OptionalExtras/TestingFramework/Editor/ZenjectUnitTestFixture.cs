using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModestTree.Util;
using Zenject;
using NUnit.Framework;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests
{
    // Inherit from this and mark you class with [TestFixture] attribute to do some unit tests
    public abstract class ZenjectUnitTestFixture
    {
        DiContainer _container;

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        [SetUp]
        public virtual void Setup()
        {
            _container = new DiContainer(false);
            InstallBindings();

            _container.Validate();
            _container.Inject(this);

            foreach (var initializable in Container.ResolveAll<IInitializable>(true))
            {
                initializable.Initialize();
            }
        }

        [TearDown]
        public virtual void Destroy()
        {
            foreach (var disposable in Container.ResolveAll<IDisposable>(true))
            {
                disposable.Dispose();
            }

            _container = null;
        }

        public virtual void InstallBindings()
        {
            // Optional
        }
    }
}
