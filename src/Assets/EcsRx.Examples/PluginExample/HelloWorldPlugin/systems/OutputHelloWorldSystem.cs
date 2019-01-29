using EcsRx.Entities;
using EcsRx.Examples.PluginExample.HelloWorldPlugin.components;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using UnityEngine;

namespace EcsRx.Examples.PluginExample.HelloWorldPlugin.systems
{
    public class OutputHelloWorldSystem : ISetupSystem
    {
        public IGroup Group => new Group(typeof(SayHelloWorldComponent));

        public void Setup(IEntity entity)
        {
            Debug.Log($"Entity {entity.Id} Says Hello World");
        }
    }
}