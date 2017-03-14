using System.Collections.Generic;
using System.Text;

namespace Tests.Editor.Helpers.Mapping
{
    public static class DebugSerializer
    {
        public static string SerializeData<T>(this TypePropertyMappings typePropertyMappings, T data)
        {
            var output = new StringBuilder();

            var result = Serialize(typePropertyMappings.Mappings, data);
            output.AppendLine(result);
            return output.ToString();
        }

        private static string SerializeProperty<T>(this PropertyMapping propertyMapping, T data)
        {
            var output = propertyMapping.GetValue(data);
            return string.Format("{0} : {1}", propertyMapping.ScopedName, output);
        }

        private static string SerializeNestedObject<T>(this NestedMapping nestedMapping, T data)
        {
            var output = new StringBuilder();
            var currentData = nestedMapping.GetValue(data);
            var result = Serialize(nestedMapping.InternalMappings, currentData);
            output.Append(result);
            return output.ToString();
        }

        private static string Serialize<T>(IEnumerable<Mapping> mappings, T data)
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
                    output.Append(result);
                }
                else
                {
                    var result = SerializeCollection((mapping as CollectionPropertyMapping), data);
                    output.Append(result);
                }
            }

            return output.ToString();
        }

        private static string SerializeCollection<T>(this CollectionPropertyMapping collectionMapping, T data)
        {
            var output = new StringBuilder();
            var collectionValue = collectionMapping.GetValue(data);
            output.AppendFormat("{0} : {1} \n", collectionMapping.ScopedName, collectionValue.Count);

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];
                if (collectionMapping.InternalMappings.Count > 0)
                {
                    var result = Serialize(collectionMapping.InternalMappings, currentData);
                    output.Append(result);
                }
                else
                {
                    output.AppendFormat("{0} : {1} \n", collectionMapping.ScopedName + ".value", currentData);
                }
            }

            return output.ToString();
        }
    }
}