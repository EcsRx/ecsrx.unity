using EcsRx.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.Examples.RandomReactions.Components
{
    public class RandomColorComponent : IComponent
    {
        public ReactiveProperty<Color> Color { get; }
        public float Elapsed { get; set; }
        public float NextChangeIn { get; set; }

        public RandomColorComponent()
        {
            Color = new ReactiveProperty<Color>();
        }
    }
}