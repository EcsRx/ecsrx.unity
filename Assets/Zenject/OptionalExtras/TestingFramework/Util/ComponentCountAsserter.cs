using System;
using Zenject;
using UnityEngine;
using System.Linq;
using ModestTree;

namespace Zenject.TestFramework
{
    public class ComponentCountAsserter<TComponent> : IInitializable
    {
        readonly int _expectedCount;

        public ComponentCountAsserter(int expectedCount)
        {
            _expectedCount = expectedCount;
        }

        public void Initialize()
        {
            Assert.That(typeof(TComponent).DerivesFromOrEqual<Component>() || typeof(TComponent).IsAbstract());

            var num = GameObject.FindObjectsOfType<Transform>()
                .SelectMany(x => x.gameObject.GetComponents<TComponent>()).Count();

            Assert.That(num == _expectedCount,
                "Expected to find '{0}' components of type '{1}' but instead found '{2}'",
                _expectedCount, typeof(TComponent).Name(), num);

            Log.Info("Correctly detected '{0}' components of type '{1}'",
                _expectedCount, typeof(TComponent).Name());
        }
    }
}
