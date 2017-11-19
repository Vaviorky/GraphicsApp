using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace GraphicsProject.Classes.Histograms
{
    public static class BrushHistogramExtensions
    {
        private static readonly BrushHelper Bh = new BrushHelper();

        public static async void EqualizeHistogram(this Brush brush)
        {
            Bh.Initialize(brush);
            var histogram = new Histogram((ImageBrush)brush);

            var maxPixels = Bh.Width * Bh.Height;

            var dystR = HistogramHelper.GetDystrybuanta(histogram.HistogramR, maxPixels);
            var dystG = HistogramHelper.GetDystrybuanta(histogram.HistogramG, maxPixels);
            var dystB = HistogramHelper.GetDystrybuanta(histogram.HistogramB, maxPixels);

            var lutR = HistogramHelper.GetLutEqualization(dystR);
            var lutG = HistogramHelper.GetLutEqualization(dystG);
            var lutB = HistogramHelper.GetLutEqualization(dystB);

            for (var i = 0; i < Bh.Length; i += 4)
            {
                var rValue = Bh.Pixels[i + 2];
                var gValue = Bh.Pixels[i + 1];
                var bValue = Bh.Pixels[i];

                Bh.Pixels[i + 2] = (byte)lutR[rValue];
                Bh.Pixels[i + 1] = (byte)lutG[gValue];
                Bh.Pixels[i] = (byte)lutB[bValue];
            }

            await Bh.UpdateBrush();
        }
    }
}
