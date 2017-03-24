using System;

namespace Persistity.Exceptions
{
    public class TypeNotPersistable : Exception
    {
        public TypeNotPersistable(Type type) : base(string.Format("{0} is not persistable, ensure it has a Persist attribute", type))
        {}
    }
}