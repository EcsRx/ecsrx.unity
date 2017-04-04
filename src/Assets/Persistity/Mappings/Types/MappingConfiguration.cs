using System;
using System.Collections.Generic;

namespace Persistity.Mappings.Types
{
    public class TypeAnalyzerConfiguration
    {
        public IEnumerable<Type> TreatAsPrimitives { get; set; }
        public IEnumerable<Type> IgnoredTypes { get; set; }

        public static TypeAnalyzerConfiguration Default
        {
            get
            {
                return new TypeAnalyzerConfiguration
                {
                    TreatAsPrimitives = new Type[0],
                    IgnoredTypes = new Type[0]
                };
            }
        }
    }
}