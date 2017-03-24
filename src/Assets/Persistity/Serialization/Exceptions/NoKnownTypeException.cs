using System;

namespace Persistity.Serialization.Exceptions
{
    public class NoKnownTypeException : Exception
    {
        public NoKnownTypeException(Type type) : base(string.Format("{0} is not a known type", type))
        {}
    }
}