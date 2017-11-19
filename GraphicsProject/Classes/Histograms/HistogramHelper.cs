using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace GraphicsProject.Classes.Histograms
{
    public static class HistogramHelper
    {
        public static PointCollection ConvertToPointCollection(int[] values)
        {
            var max = values.Max();

            var points = new PointCollection();

            points.Add(new Point(0, max));

            for (var i = 0; i < values.Length; i++)
            {
                points.Add(new Point(i, max - values[i]));
            }

            points.Add(new Point(values.Length - 1, max));

            return points;
        }

        public static double[] GetDystrybuanta(int[] histogram, int maxPixels)
        {
            double suma = 0;
            var dystrybuanta = new double[histogram.Length];

            Debug.WriteLine(maxPixels);

            for (var i = 0; i < histogram.Length; i++)
            {
                suma += histogram[i];
                dystrybuanta[i] = suma / maxPixels;
            }
            return dystrybuanta;
        }

        public static int[] GetLutEqualization(double[] dystrybuanta)
        {
            var i = 0;
            while (dystrybuanta[i] == 0)
            {
                i++;
            }

            var nonzero = dystrybuanta[i];
            var lut = new int[256];

            for (i = 0; i < 256; i++)
            {
                lut[i] = (int)((dystrybuanta[i] - nonzero) / (1 - nonzero) * 255);

                if (lut[i] > 255)
                {
                    lut[i] = 255;
                }

                if (lut[i] < 0)
                {
                    lut[i] = 0;
                }
            }
            return lut;
        }
    }
}
