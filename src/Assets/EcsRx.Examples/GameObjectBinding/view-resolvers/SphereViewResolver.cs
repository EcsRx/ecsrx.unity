using Assets.EcsRx.Examples.GameObjectBinding.components;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.EcsRx.Examples.GameObjectBinding
{
    public class SphereViewResolver : ViewResolverSystem
    {
        public override Group TargetGroup
        {
            get { return base.TargetGroup.WithComponent<SphereComponent>(); }
        }

        public SphereViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {}
        
        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            view.transform.position = new Vector3(2,0,0);
            return view;
        }
    }
}