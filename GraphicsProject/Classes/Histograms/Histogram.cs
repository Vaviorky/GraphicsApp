using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphicsProject.Classes.Histograms
{
    public class Histogram
    {
        public int[] HistogramR { get; private set; }
        public int[] HistogramG { get; private set; }
        public int[] HistogramB { get; private set; }
        public int[] HistogramAv { get; private set; }

        public int HistogramRMax => HistogramR.Max();
        public int HistogramGMax => HistogramG.Max();
        public int HistogramBMax => HistogramB.Max();
        public int HistogramAvMax => HistogramAv.Max();

        private readonly byte[] _pixels;

        public Histogram(ImageBrush image)
        {
            _pixels = ((WriteableBitmap)image.ImageSource).PixelBuffer.ToArray();
            CalculateHistograms();
        }

        private void CalculateHistograms()
        {
            HistogramR = new int[256];
            HistogramG = new int[256];
            HistogramB = new int[256];
            HistogramAv = new int[256];

            for (var i = 0; i < _pixels.Length; i += 4)
            {
                var r = _pixels[i + 2];
                var g = _pixels[i + 1];
                var b = _pixels[i];

                HistogramR[r]++;
                HistogramG[g]++;
                HistogramB[b]++;

                var temp = (r + g + b) / 3;
                HistogramAv[temp]++;
            }
        }
    }
}
