using UnityEngine;

namespace EcsRx.Unity.MonoBehaviours.Editor.Extensions
{
    public static class IntExtensions
    {
        public static Color ToMutedColor(this int color, float saturation = 0.20f)
        {
            var rInt = (color >> 16) & 0xff;
            var r = rInt == 0 ? 0.0f : rInt / 255.0f;
            return Color.HSVToRGB(r, saturation, 0.75f);
        }
        
        public static Color ToColor(this int color, float alpha = 1.0f)
        {
            var rInt = (color >> 16) & 0xff;
            var r = rInt == 0 ? 0.0f : rInt / 255.0f;
            var gInt = (color >> 8) & 0xff;
            var g = gInt == 0 ? 0.0f : gInt / 255.0f;
            var bInt = (color >> 0) & 0xff;
            var b = bInt == 0 ? 0.0f : bInt / 255.0f;

            return new Color(r, g, b, alpha);
        }

        public static Color Desaturate2(this Color color, float saturation = 0.25f)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return Color.HSVToRGB(h, saturation, v);
        }

        public static Color Desaturate(this Color color, float saturation = 0.25f)
        {
            var lightness = 0.3f*color.r + 0.6f*color.g + 0.1f*color.b;
            var r = color.r + saturation * (lightness - color.r);
            var g = color.g + saturation * (lightness - color.g);
            var b = color.b + saturation * (lightness - color.b);
            return new Color(r,g,b, color.a);
        }
    }
}
