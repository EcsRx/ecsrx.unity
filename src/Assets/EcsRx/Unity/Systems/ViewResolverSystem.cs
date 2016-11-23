using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class ViewResolverSystem : ISetupSystem
    {
        public IViewHandler ViewHandler { get; private set; }

        public virtual IGroup TargetGroup
        {
            get { return new Group(typeof(ViewComponent)); }
        }

        protected ViewResolverSystem(IViewHandler viewHandler)
        {
            ViewHandler = viewHandler;
        }

        public abstract GameObject ResolveView(IEntity entity);

        public void Setup(IEntity entity)
        {
            ViewHandler.SetupView(entity, ResolveView);
        }
    }
}