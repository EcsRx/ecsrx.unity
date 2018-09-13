using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Editor.Extensions
{
    public static class IntExtensions
    {
        public static Color ToColor(this int color, float alpha)
        {
            var rInt = (color >> 16) & 0xff;
            var r = rInt == 0 ? 0.0f : rInt / 255.0f;
            var gInt = (color >> 8) & 0xff;
            var g = gInt == 0 ? 0.0f : gInt / 255.0f;
            var bInt = (color >> 0) & 0xff;
            var b = bInt == 0 ? 0.0f : bInt / 255.0f;

            return new Color(r, g, b, alpha);
        }
    }
}
