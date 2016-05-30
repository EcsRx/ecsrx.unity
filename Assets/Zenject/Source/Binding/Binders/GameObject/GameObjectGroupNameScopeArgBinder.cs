using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class GameObjectGroupNameScopeArgBinder : ScopeArgBinder
    {
        public GameObjectGroupNameScopeArgBinder(
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

        public ScopeArgBinder UnderGameObjectGroup(string gameObjectGroupName)
        {
            GameObjectInfo.GroupName = gameObjectGroupName;
            return this;
        }
    }
}
