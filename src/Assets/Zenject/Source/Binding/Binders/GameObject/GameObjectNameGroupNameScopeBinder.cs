using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class GameObjectNameGroupNameScopeBinder : GameObjectGroupNameScopeBinder
    {
        public GameObjectNameGroupNameScopeBinder(
            BindInfo bindInfo,
            GameObjectBindInfo gameObjectInfo)
            : base(bindInfo, gameObjectInfo)
        {
        }

        public GameObjectGroupNameScopeBinder WithGameObjectName(string gameObjectName)
        {
            GameObjectInfo.Name = gameObjectName;
            return this;
        }
    }
}
