using System;

namespace Persistity.Mappings
{
    public class PropertyMapping : Mapping
    {
        public Func<object, object> GetValue { get; set; }
        public Action<object, object> SetValue { get; set; }
    }
}