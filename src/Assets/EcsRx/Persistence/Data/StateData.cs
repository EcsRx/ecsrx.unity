using System;
using UnityEngine;

namespace EcsRx.Persistence.Data
{
    [Serializable]
    public class StateData
    {
        [SerializeField]
        private string _state;

        public StateData()
        {}

        public StateData(string state)
        { _state = state; }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}