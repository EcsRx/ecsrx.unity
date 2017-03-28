using System.Text.RegularExpressions;
using Persistity;
using Persistity.Processors;

namespace EcsRx.Persistence.Processors
{
    public class MakeComponentStateStringProcessor : IProcessor
    {
        private Regex _regexPattern = new Regex("{\"ComponentState\":.*}(.){1}");

        public DataObject Process(DataObject data)
        {
            var stringifiedCompnentState = data.AsString
                .Replace("\"ComponentState\":{", "\"ComponentState\":\"{");
            
            stringifiedCompnentState = _regexPattern.Replace(stringifiedCompnentState, "\",");

            return new DataObject(stringifiedCompnentState);
        }
    }
}