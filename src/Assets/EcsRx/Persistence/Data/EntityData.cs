using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EcsRx.Persistence.Data
{
    [Serializable]
    public class EntityData
    {
        [SerializeField]
        private Guid _entityId;

        [SerializeField]
        private IList<ComponentData> _componentData;
     
        public Guid EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
        }

        public IList<ComponentData> ComponentData
        {
            get { return _componentData; }
            set { _componentData = value; }
        }

        public EntityData()
        {
            _componentData = new List<ComponentData>();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Entity Id: " + EntityId);

            foreach (var componentData in ComponentData)
            {
                stringBuilder.AppendLine(componentData.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}