using System.Collections.Generic;
using EcsRx.Entities;

namespace EcsRx.Unity.MonoBehaviours.Models
{
    public class EntityImportWrapper
    {
        public List<string> ComponentTypes { get; set; }
        public IEntity Entity { get; set; }
    }
}