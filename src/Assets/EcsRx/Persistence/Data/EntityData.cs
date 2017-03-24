using System;
using System.Collections.Generic;
using System.Text;

namespace EcsRx.Persistence.Data
{
    public class EntityData
    {
        public Guid EntityId { get; set; }
        public IList<ComponentData> ComponentData { get; set; }

        public EntityData()
        {
            ComponentData = new List<ComponentData>();
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