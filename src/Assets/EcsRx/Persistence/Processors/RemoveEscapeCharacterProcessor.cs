using System.Text.RegularExpressions;
using Persistity;
using Persistity.Processors;

namespace EcsRx.Persistence.Processors
{
    public class RemoveEscapeCharacterProcessor : IProcessor
    {
        public DataObject Process(DataObject data)
        {
            var removedEscapeCharacters = Regex.Unescape(data.AsString);
            return new DataObject(removedEscapeCharacters);
        }
    }
}