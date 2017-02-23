using System.Collections.Generic;

namespace EcsRx.Persistence.Data
{
    public class ApplicationData
    {
        public IList<EntityData> EntityData { get; set; }

        public ApplicationData()
        {
            EntityData = new List<EntityData>();
        }
    }
}