using System.Collections.Generic;
using System.Text;

namespace Tests.Editor.Helpers.Mapping
{
    public static class DebugSerializer
    {
        public static string SerializeData<T>(this TypePropertyMappings typePropertyMappings, T data)
        {
            var output = new StringBuilder();

            var result = Blah(typePropertyMappings.Mappings, data);
            output.AppendLine(result);
            return output.ToString();
        }

        private static string SerializeProperty<T>(this PropertyMapping propertyMapping, T data)
        {
            var output = propertyMapping.GetValue(data);
            return string.Format("{0} : {1}, \n", propertyMapping.ScopedName, output);
        }

        private static string SerializeNestedObject<T>(this NestedMapping nestedMapping, T data)
        {
            var output = new StringBuilder();
            var currentData = nestedMapping.GetValue(data);
            var result = Blah(nestedMapping.InternalMappings, currentData);
            output.AppendLine(result);
            return output.ToString();
        }

        private static string Blah<T>(IEnumerable<Mapping> mappings, T data)
        {
            var output = new StringBuilder();

            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var result = SerializeProperty((mapping as PropertyMapping), data);
                    output.AppendLine(result);
                }
                else if (mapping is NestedMapping)
                {
                    var result = SerializeNestedObject((mapping as NestedMapping), data);
                    output.AppendLine(result);
                }
                else
                {
                    var result = SerializeCollection((mapping as CollectionPropertyMapping), data);
                    output.AppendLine(result);
                }
            }

            return output.ToString();
        }

        private static string SerializeCollection<T>(this CollectionPropertyMapping collectionMapping, T data)
        {
            var output = new StringBuilder();
            var arrayValue = collectionMapping.GetValue(data);
            output.AppendFormat("{0} : {1}, \n", collectionMapping.ScopedName, arrayValue.Length);

            for (var i = 0; i < arrayValue.Length; i++)
            {
                var currentData = arrayValue.GetValue(i);
                var result = Blah(collectionMapping.InternalMappings, currentData);
                output.AppendLine(result);
            }

            return output.ToString();
        }
    }
}