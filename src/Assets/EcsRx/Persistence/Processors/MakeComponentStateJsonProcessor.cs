using Persistity;
using Persistity.Processors;

namespace EcsRx.Persistence.Processors
{
    public class MakeComponentStateJsonProcessor : IProcessor
    {
        public DataObject Process(DataObject data)
        {
            var jsonifiedOutput = data.AsString
                .Replace("\"{", "{")
                .Replace("}\"", "}");

            return new DataObject(jsonifiedOutput);
        }
    }
}