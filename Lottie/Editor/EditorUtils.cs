using UnityEngine;

namespace Lottie.Editor
{
    internal static class EditorUtils
    {
        public static Texture2D CreateRoundedTexture(int width, int height, int radius, Color color, bool roundTopLeft = true, bool roundTopRight = true, bool roundBottomLeft = true, bool roundBottomRight = true)
        {
            var roundedTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var colors = new Color[width * height];

            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }

            if (roundTopLeft) ProcessCorner(colors, width, height, 0, 0, radius, false, false);
            if (roundTopRight) ProcessCorner(colors, width, height, width - radius, 0, radius, true, false);
            if (roundBottomLeft) ProcessCorner(colors, width, height, 0, height - radius, radius, false, true);
            if (roundBottomRight) ProcessCorner(colors, width, height, width - radius, height - radius, radius, true, true);

            roundedTexture.SetPixels(colors);
            roundedTexture.Apply();
            return roundedTexture;
        }

        private static void ProcessCorner(Color[] colors, int width, int height, int startX, int startY, int radius, bool flipX, bool flipY)
        {
            for (var y = 0; y < radius; y++)
            {
                for (var x = 0; x < radius; x++)
                {
                    var distance = Mathf.Sqrt(x * x + y * y);
                    if (!(distance > radius)) continue;
                    var pixelX = !flipX ? startX + (radius - x - 1) : startX + x;
                    var pixelY = !flipY ? startY + (radius - y - 1) : startY + y;
                    colors[pixelY * width + pixelX] = Color.clear;
                }
            }
        }
    }
}