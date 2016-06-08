using Assets.EcsRx.Examples.ViewBinding.Components;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace Assets.EcsRx.Examples.ViewBinding.ViewResolvers
{
    public class SphereViewResolver : ViewResolverSystem
    {
        private readonly Transform ParentTrasform = GameObject.Find("Entities").transform;

        public override IGroup TargetGroup
        {
            get { return base.TargetGroup.WithComponent<SphereComponent>(); }
        }

        public SphereViewResolver(IPoolManager poolManager) : base(poolManager) { }

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            view.transform.position = new Vector3(2,0,0);
            view.transform.parent = ParentTrasform;
            return view;
        }
    }
}
