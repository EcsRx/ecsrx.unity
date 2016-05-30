using System;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class GameObjectNameGroupNameBinder : GameObjectGroupNameBinder
    {
        public GameObjectNameGroupNameBinder(
            BindInfo bindInfo, GameObjectBindInfo gameObjectInfo)
            : base(bindInfo, gameObjectInfo)
        {
        }

        public GameObjectGroupNameBinder WithGameObjectName(string gameObjectName)
        {
            GameObjectInfo.Name = gameObjectName;
            return this;
        }
    }
}
