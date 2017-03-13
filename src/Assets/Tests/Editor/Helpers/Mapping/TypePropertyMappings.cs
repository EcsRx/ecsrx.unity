using System.Collections.Generic;

namespace Tests.Editor.Helpers.Mapping
{
    public class TypePropertyMappings
    {
        public string Name { get; set; }
        public List<Mapping> Mappings { get; private set; }

        public TypePropertyMappings()
        {
            Mappings = new List<Mapping>();
        }
    }
}