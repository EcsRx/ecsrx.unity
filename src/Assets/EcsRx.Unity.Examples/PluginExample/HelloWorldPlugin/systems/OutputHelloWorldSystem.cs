using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Examples.PluginExample.HelloWorldPlugin.components;
using UnityEngine;

namespace EcsRx.Unity.Examples.PluginExample.HelloWorldPlugin.systems
{
    public class OutputHelloWorldSystem : ISetupSystem
    {
        public IGroup TargetGroup { get { return new Group(typeof(SayHelloWorldComponent));} }

        public void Setup(IEntity entity)
        {
            Debug.Log(string.Format("Entity {0} Says Hello World", entity.Id));
        }
    }
}