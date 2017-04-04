using System;
using EcsRx.Persistence.Data;
using Persistity.Json;
using Persistity.Serialization;

namespace EcsRx.Persistence.TypeHandlers
{
    public class JsonStateDataHandler : ITypeHandler<JSONNode, JSONNode>
    {
        public bool MatchesType(Type type)
        { return type == typeof(StateData); }

        public void HandleTypeSerialization(JSONNode state, object data)
        {
            var typedData = (StateData)data;
            var jsonData = JSON.Parse(typedData.State);
            state.Add("Data", jsonData);
        }

        public object HandleTypeDeserialization(JSONNode state)
        {
            var data = state["Data"];
            return new StateData(data.ToString());
        }
    }
}