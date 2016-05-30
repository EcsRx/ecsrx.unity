using System;
using Zenject;
using UnityEngine;
using System.Linq;
using ModestTree;

namespace Zenject.TestFramework
{
    public class GameObjectNameCountAsserter : IInitializable
    {
        readonly GameObject _context;
        readonly int _expectedCount;
        readonly string _name;

        public GameObjectNameCountAsserter(
            string name,
            int expectedCount,
            Context context)
        {
            _name = name;
            _context = context.gameObject;
            _expectedCount = expectedCount;
        }

        public void Initialize()
        {
            Assert.IsEqual(
                _context.transform.Cast<Transform>()
                    .Where(x => x.name == _name).Count(),
                _expectedCount);

            Log.Info("Correctly detected '{0}' game objects with name '{1}'", _expectedCount, _name);
        }
    }
}


