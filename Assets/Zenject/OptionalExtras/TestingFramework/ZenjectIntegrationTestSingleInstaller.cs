using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModestTree.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Zenject;
using ModestTree;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zenject.TestFramework
{
    public class ZenjectIntegrationTestSingleInstaller : MonoInstaller
    {
        public ZenjectIntegrationTestFixture Fixture
        {
            get;
            set;
        }

        public MethodInfo TestMethod
        {
            get;
            set;
        }

        public override void InstallBindings()
        {
            Fixture.Container = Container;

            TestMethod.Invoke(Fixture, new object[0]);
        }
    }
}
