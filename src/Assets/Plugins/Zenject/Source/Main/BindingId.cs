using System;

namespace Zenject
{
    [System.Diagnostics.DebuggerStepThrough]
    public class BindingId : IEquatable<BindingId>
    {
        Type _type;
        object _identifier;

        public BindingId()
        {
        }

        public BindingId(Type type, object identifier)
        {
            _type = type;
            _identifier = identifier;
        }

        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public object Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + _type.GetHashCode();
                hash = hash * 29 + (_identifier == null ? 0 : _identifier.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is BindingId)
            {
                BindingId otherId = (BindingId)other;
                return otherId == this;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(BindingId that)
        {
            return this == that;
        }

        public static bool operator ==(BindingId left, BindingId right)
        {
            return left.Type == right.Type && object.Equals(left.Identifier, right.Identifier);
        }

        public static bool operator !=(BindingId left, BindingId right)
        {
            return !left.Equals(right);
        }
    }
}
