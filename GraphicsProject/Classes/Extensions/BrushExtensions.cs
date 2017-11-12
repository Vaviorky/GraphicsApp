using System;
using Windows.UI.Xaml.Media;

namespace GraphicsProject.Classes.Extensions
{
    public static class BrushExtensions
    {
        private static readonly BrushHelper bh = new BrushHelper();

        public static async void AddPixelByValue(this Brush brush, float value)
        {
            bh.Initialize(brush);

            for (int i = 0; i < bh.Length; i += 4)
            {
                var r = Truncate(bh.Pixels[i + 2] + value);
                var g = Truncate(bh.Pixels[i + 1] + value);
                var b = Truncate(bh.Pixels[i] + value);

                bh.Pixels[i + 2] = r;
                bh.Pixels[i + 1] = g;
                bh.Pixels[i] = b;
            }

            await bh.UpdateBrush();
        }

        public static async void MultiplyPixelByValue(this Brush brush, float value)
        {
            bh.Initialize(brush);

            for (int i = 0; i < bh.Length; i += 4)
            {
                var r =  Truncate(bh.Pixels[i + 2] * value);
                var g = Truncate(bh.Pixels[i + 1] * value);
                var b = Truncate(bh.Pixels[i] * value);

                bh.Pixels[i + 2] = r;
                bh.Pixels[i + 1] = g;
                bh.Pixels[i] = b;
            }

            await bh.UpdateBrush();
        }

        public static async void MakeGrayscale_FirstWay(this Brush brush)
        {
            bh.Initialize(brush);

            for (int i = 0; i < bh.Length; i += 4)
            {
                var r = bh.Pixels[i + 2];
                var g = bh.Pixels[i + 1];
                var b = bh.Pixels[i];

                var s = (byte)Math.Round((r + g + b) / 3.0, 0, MidpointRounding.AwayFromZero);

                bh.Pixels[i + 2] = s;
                bh.Pixels[i + 1] = s;
                bh.Pixels[i] = s;
            }
            await bh.UpdateBrush();
        }

        public static async void MakeGrayscale_SecondWay(this Brush brush)
        {
            bh.Initialize(brush);

            for (int i = 0; i < bh.Length; i += 4)
            {
                var r = bh.Pixels[i + 2];
                var g = bh.Pixels[i + 1];
                var b = bh.Pixels[i];

                var gray = (r * 0.3 + g * 0.59 + b * 0.11);

                bh.Pixels[i + 2] = (byte) gray;
                bh.Pixels[i + 1] = (byte) gray;
                bh.Pixels[i] = (byte) gray;
            }
            await bh.UpdateBrush();
        }

        public static async void AdjustBrightness(this Brush brush, float brightness)
        {
            bh.Initialize(brush);

            for (int i = 0; i < bh.Length; i += 4)
            {
                var r = bh.Pixels[i + 2];
                var g = bh.Pixels[i + 1];
                var b = bh.Pixels[i];

                bh.Pixels[i + 2] = Truncate(r + (255f * brightness));
                bh.Pixels[i + 1] = Truncate(g + (255f * brightness));
                bh.Pixels[i] = Truncate(b + (255f * brightness));
            }
            await bh.UpdateBrush();
        }

        private static byte Truncate(float value)
        {
            if (value > 255)
            {
                value = 255;
            }
            else if (value < 0)
            {
                value = 0;
            }

            return (byte)value;
        }

    }
}
