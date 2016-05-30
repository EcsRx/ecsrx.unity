using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class GameObjectGroupNameBinder : ConditionBinder
    {
        public GameObjectGroupNameBinder(BindInfo bindInfo, GameObjectBindInfo gameObjInfo)
            : base(bindInfo)
        {
            GameObjectInfo = gameObjInfo;
        }

        protected GameObjectBindInfo GameObjectInfo
        {
            get;
            private set;
        }

        public ConditionBinder UnderGameObjectGroup(string gameObjectGroupName)
        {
            GameObjectInfo.GroupName = gameObjectGroupName;
            return this;
        }
    }
}
