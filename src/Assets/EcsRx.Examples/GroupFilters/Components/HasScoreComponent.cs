using System;
using EcsRx.Components;
using UniRx;

namespace Assets.EcsRx.Examples.GroupFilters.Components
{
    public class HasScoreComponent : IComponent, IDisposable
    {
        public string Name { get; set; }
        public ReactiveProperty<int> Score { get; set; }

        public HasScoreComponent()
        { Score = new IntReactiveProperty(0); }

        public void Dispose()
        {
            Score.Dispose();
        }
    }
}