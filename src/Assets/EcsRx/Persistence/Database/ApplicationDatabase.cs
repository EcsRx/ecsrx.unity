using System.Collections.Generic;
using EcsRx.Persistence.Data;

namespace EcsRx.Persistence.Database
{
    public class ApplicationDatabase
    {
        public IList<PoolData> Pools { get; set; }
        public string Version { get; set; }
        
        public ApplicationDatabase()
        {
            Pools = new List<PoolData>();
            Version = "1.0.0";
        }
    }
} 