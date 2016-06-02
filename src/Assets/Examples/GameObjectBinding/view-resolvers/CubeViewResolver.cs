using Assets.Examples.GameObjectBinding.components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace Assets.Examples.GameObjectBinding
{
    public class CubeViewResolver : ViewResolverSystem
    {
        public override IGroup TargetGroup
        {
            get { return base.TargetGroup.WithComponent<CubeComponent>(); }
        }

        public CubeViewResolver(IPoolManager poolManager) : base(poolManager) {}

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.transform.position = new Vector3(-2, 0, 0);
            return view;
        }
    }
}