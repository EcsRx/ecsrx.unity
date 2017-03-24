using System;
using System.Text;
using UnityEngine;

namespace EcsRx.Persistence.Data
{
    [Serializable]
    public class ComponentData
    {
        [SerializeField]
        private string _componentTypeReference;

        [SerializeField]
        private byte[] _componentState;

        public string ComponentTypeReference
        {
            get { return _componentTypeReference; }
            set { _componentTypeReference = value; }
        }

        public byte[] ComponentState
        {
            get { return _componentState; }
            set { _componentState = value; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", _componentTypeReference, Encoding.Default.GetString(_componentState));
        }
    }
}