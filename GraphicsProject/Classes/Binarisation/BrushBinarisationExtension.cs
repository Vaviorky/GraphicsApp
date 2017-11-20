using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace GraphicsProject.Classes.Binarisation
{
    public static class BrushBinarisationExtension
    {
        private static readonly BrushHelper Bh = new BrushHelper();

        public static bool IsGrayscaled(this Brush brush)
        {
            Bh.Initialize(brush);

            for (var i = 0; i < Bh.Length; i += 4)
            {
                var r = Bh.Pixels[i + 2];
                var g = Bh.Pixels[i + 1];
                var b = Bh.Pixels[i];

                if (r != g || r != b || g != b)
                {
                    return false;
                }
            }
            return true;
        }

        public static async void MakeBinarisation(this Brush brush, int threshold)
        {
            Bh.Initialize(brush);

            for (var i = 0; i < Bh.Length; i += 4)
            {
                var oldPixelValue = Bh.Pixels[i];
                var value = oldPixelValue <= threshold ? 0 : 255;

                Bh.Pixels[i + 2] = (byte)value;
                Bh.Pixels[i + 1] = (byte)value;
                Bh.Pixels[i] = (byte)value;
            }

            await Bh.UpdateBrush();
        }
    }
}
