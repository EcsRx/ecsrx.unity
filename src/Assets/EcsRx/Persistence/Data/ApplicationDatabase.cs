using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EcsRx.Persistence.Data
{
    [Serializable]
    public class ApplicationDatabase
    {
        [SerializeField]
        private IList<EntityData> _entityData;

        [SerializeField]
        private string _version = "1.0.0";

        public IList<EntityData> EntityData
        {
            get { return _entityData; }
            set { _entityData = value; }
        }

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public ApplicationDatabase()
        {
            _entityData = new List<EntityData>();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var entityData in _entityData)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(entityData.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}