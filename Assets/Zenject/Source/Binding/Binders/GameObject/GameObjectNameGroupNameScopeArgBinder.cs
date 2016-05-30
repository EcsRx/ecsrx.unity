using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class GameObjectNameGroupNameScopeArgBinder : GameObjectGroupNameScopeArgBinder
    {
        public GameObjectNameGroupNameScopeArgBinder(
            BindInfo bindInfo,
            GameObjectBindInfo gameObjectInfo)
            : base(bindInfo, gameObjectInfo)
        {
        }

        public GameObjectGroupNameScopeArgBinder WithGameObjectName(string gameObjectName)
        {
            GameObjectInfo.Name = gameObjectName;
            return this;
        }
    }
}
