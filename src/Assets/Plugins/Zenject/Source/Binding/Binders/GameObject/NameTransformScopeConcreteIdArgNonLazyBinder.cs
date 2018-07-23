#if !NOT_UNITY3D

namespace Zenject
{
    public class NameTransformScopeConcreteIdArgNonLazyBinder : TransformScopeConcreteIdArgNonLazyBinder
    {
        public NameTransformScopeConcreteIdArgNonLazyBinder(
            BindInfo bindInfo,
            GameObjectCreationParameters gameObjectInfo)
            : base(bindInfo, gameObjectInfo)
        {
        }

        public TransformScopeConcreteIdArgNonLazyBinder WithGameObjectName(string gameObjectName)
        {
            GameObjectInfo.Name = gameObjectName;
            return this;
        }
    }
}

#endif

