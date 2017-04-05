using System;
using UnityEngine;

namespace EcsRx.Persistence.Database
{
    [Serializable]
    public class ApplicationEntityLink
    {
        [SerializeField]
        private Guid _entityId;

        [SerializeField]
        private byte[] _entityData;
        
        public Guid EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        public byte[] EntityData
        {
            get { return _entityData; }
            set { _entityData = value; }
        }
    }
}