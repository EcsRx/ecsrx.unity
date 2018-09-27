using System;
using EcsRx.Components;
using UniRx;

namespace EcsRx.Examples.SceneFirstSetup.Components
{
    public class TestComponent : IComponent, IDisposable
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsHappy { get; set; }
        public float Roundness { get; set; }
        public ReactiveProperty<float> ReactiveFloat { get; set; }

        public TestComponent()
        {
            ReactiveFloat = new ReactiveProperty<float>(0);
        }

        public void Dispose()
        {
            ReactiveFloat?.Dispose();
        }
    }
}