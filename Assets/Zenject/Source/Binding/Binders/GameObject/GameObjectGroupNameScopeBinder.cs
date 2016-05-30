using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class GameObjectGroupNameScopeBinder : ScopeBinder
    {
        public GameObjectGroupNameScopeBinder(
            BindInfo bindInfo,
            GameObjectBindInfo gameObjectInfo)
            : base(bindInfo)
        {
            GameObjectInfo = gameObjectInfo;
        }

        protected GameObjectBindInfo GameObjectInfo
        {
            get;
            private set;
        }

        public ScopeBinder UnderGameObjectGroup(string gameObjectGroupName)
        {
            GameObjectInfo.GroupName = gameObjectGroupName;
            return this;
        }
    }
}
