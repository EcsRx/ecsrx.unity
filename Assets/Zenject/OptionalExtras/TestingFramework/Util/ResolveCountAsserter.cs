using System;
using Zenject;
using UnityEngine;
using System.Collections.Generic;
using ModestTree;

namespace Zenject.TestFramework
{
    public class ResolveCountAsserter<TContract> : IInitializable
    {
        readonly int _expectedCount;
        readonly List<TContract> _results;

        public ResolveCountAsserter(
            [InjectOptional]
            List<TContract> results,
            int expectedCount)
        {
            _results = results;
            _expectedCount = expectedCount;
        }

        public void Initialize()
        {
            Assert.That(_results.Count == _expectedCount,
                "Expected to find '{0}' instances of type '{1}' but instead found '{2}'",
                _expectedCount, typeof(TContract).Name(), _results.Count);

            Log.Info("Correctly resolved '{0}' objects with type '{1}'", _expectedCount, typeof(TContract).Name());
        }
    }
}

