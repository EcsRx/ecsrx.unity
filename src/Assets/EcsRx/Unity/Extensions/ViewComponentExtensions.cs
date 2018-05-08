using EcsRx.Views.Components;
using UnityEngine;

namespace EcsRx.Unity.Extensions
{
    public static class ViewComponentExtensions
    {
        public static void DestroyView(this ViewComponent viewComponent, float delay = 0.0f)
        {
            Object.Destroy((GameObject)viewComponent.View, delay);
        }
    }
}