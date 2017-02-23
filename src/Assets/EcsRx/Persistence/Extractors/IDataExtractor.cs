using System;
using System.Collections.Generic;

namespace EcsRx.Persistence.Processors
{
    public interface IDataExtractor
    {
        bool MatchesType(Type givenType);
        KeyValuePair<string, object> ProcessData<T>(T data);
    }
}