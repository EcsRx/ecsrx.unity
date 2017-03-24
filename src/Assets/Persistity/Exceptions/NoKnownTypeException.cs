using System;

namespace Persistity.Exceptions
{
    public class NoKnownTypeException : Exception
    {
        public NoKnownTypeException(Type type) : base(string.Format("{0} is not a known type", type))
        {}
    }
}