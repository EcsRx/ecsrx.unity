﻿using EcsRx.Components;
using UniRx;
using UnityEngine;

namespace Assets.Examples.RandomReactions.Components
{
    public class RandomColorComponent : IComponent
    {
        public ReactiveProperty<Color> Color { get; set; }
        public float Elapsed { get; set; }
        public float NextChangeIn { get; set; }

        public RandomColorComponent()
        {
            Color = new ReactiveProperty<Color>();
        }
    }
}