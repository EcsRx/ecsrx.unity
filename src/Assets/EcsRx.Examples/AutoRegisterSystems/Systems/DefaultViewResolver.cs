using EcsRx.Entities;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace Assets.EcsRx.Examples.AutoRegisterSystems.Systems
{
    public class DefaultViewResolver : ViewResolverSystem
    {
        public DefaultViewResolver(IViewHandler viewHandler) : base(viewHandler)
        {}

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.name = "entity-" + entity.Id;
            return view;
        }
    }
}