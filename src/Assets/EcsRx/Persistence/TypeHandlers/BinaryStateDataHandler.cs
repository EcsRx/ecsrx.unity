using System;
using System.IO;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Extensions;
using Persistity.Serialization;

namespace EcsRx.Persistence.TypeHandlers
{
    public class BinaryStateDataHandler : ITypeHandler<BinaryWriter, BinaryReader>
    {
        public bool MatchesType(Type type)
        { return type == typeof(StateData); }

        public void HandleTypeSerialization(BinaryWriter state, object data)
        {
            var typedData = (StateData)data;
            state.WriteByteArray(typedData.State);
        }

        public object HandleTypeDeserialization(BinaryReader state)
        {
            var stateData = state.ReadString();
            return new StateData(stateData);
        }
    }
}