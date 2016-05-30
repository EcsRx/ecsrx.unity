using System;
using Zenject;
using UnityEngine;
using ModestTree;

namespace Zenject.TestFramework
{
    public class GameObjectCountAsserter : IInitializable
    {
        readonly GameObject _context;
        readonly int _expectedCount;

        public GameObjectCountAsserter(
            int expectedCount,
            Context context)
        {
            _context = context.gameObject;
            _expectedCount = expectedCount;
        }

        public void Initialize()
        {
            Assert.IsEqual(_context.transform.childCount, _expectedCount);

            Log.Info("Correctly detected '{0}' game objects", _expectedCount);
        }
    }
}

