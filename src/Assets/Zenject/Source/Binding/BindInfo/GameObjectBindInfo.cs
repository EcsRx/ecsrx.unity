using System;
using System.Collections.Generic;

namespace Zenject
{
    public class GameObjectBindInfo
    {
        public string Name
        {
            get;
            set;
        }

        public string GroupName
        {
            get;
            set;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + (this.Name == null ? 0 : this.Name.GetHashCode());
                hash = hash * 29 + (this.GroupName == null ? 0 : this.GroupName.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is GameObjectBindInfo)
            {
                GameObjectBindInfo otherId = (GameObjectBindInfo)other;
                return otherId == this;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(GameObjectBindInfo that)
        {
            return this == that;
        }

        public static bool operator ==(GameObjectBindInfo left, GameObjectBindInfo right)
        {
            return object.Equals(left.Name, right.Name)
                && object.Equals(left.GroupName, right.GroupName);
        }

        public static bool operator !=(GameObjectBindInfo left, GameObjectBindInfo right)
        {
            return !left.Equals(right);
        }
    }
}

