using System;
using UnityEngine;

namespace EcsRx.Persistence.Data
{
    [Serializable]
    public class ComponentData
    {
        [SerializeField]
        private StateData _componentState;

        [SerializeField]
        private string _componentName;

        public StateData ComponentState
        {
            get { return _componentState; }
            set { _componentState = value; }
        }

        public string ComponentName
        {
            get { return _componentName; }
            set { _componentName = value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", ComponentName, _componentState.State);
        }
    }
}