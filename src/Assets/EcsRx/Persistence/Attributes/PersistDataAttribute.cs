using System;

namespace EcsRx.Persistence.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PersistDataAttribute : Attribute
    {
        public PersistDataAttribute()
        {
            
        }
    }
}