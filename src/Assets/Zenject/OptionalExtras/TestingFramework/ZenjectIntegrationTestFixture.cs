using System;
using UnityEngine;

namespace Zenject.TestFramework
{
    public abstract class ZenjectIntegrationTestFixture : MonoBehaviour
    {
        public DiContainer Container
        {
            get;
            set;
        }
    }
}
