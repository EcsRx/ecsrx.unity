using System.Collections.Generic;

namespace EcsRx.Persistence.Data
{
    public class PoolData
    {
        public string PoolName { get; set; }
        public IList<EntityData> Entities { get; set; }

        public PoolData()
        {
            Entities = new List<EntityData>();
        }
    }
}