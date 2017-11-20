using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GraphicsProject.Enums;

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

        public static int[] GetLutStretching(double min, double max)
        {
            var lut = new int[256];

            for (var i = 0; i < 256; i++)
            {
                var value = (255.0 / (max - min)) * (i - min);

                if (value > 255)
                {
                    lut[i] = 255;
                }
                else if (value < 0)
                {
                    lut[i] = 0;
                }
                else
                {
                    lut[i] = (int)Math.Round(value, 0, MidpointRounding.AwayFromZero);
                }
            }
            return lut;
        }

        public static async Task<ImageBrush> StretchHistogram(ImageBrush origin, int[] lut, HistogramType type)
        {
            var wb = (WriteableBitmap) origin.ImageSource;
            var data = wb.PixelBuffer.ToArray();

            for (var i = 0; i < data.Length; i += 4)
            {
                var r = data[i + 2];
                var g = data[i + 1];
                var b = data[i];

                switch (type)
                {
                    default:
                    case HistogramType.Red:
                        data[i + 2] = (byte)lut[r];
                        break;
                    case HistogramType.Green:
                        data[i + 1] = (byte)lut[g];
                        break;
                    case HistogramType.Blue:
                        data[i] = (byte)lut[b];
                        break;
                    case HistogramType.Average:
                        data[i + 2] = (byte)lut[r];
                        data[i + 1] = (byte)lut[g];
                        data[i] = (byte)lut[b];
                        break;
                }
            }
            var newWb = new WriteableBitmap(wb.PixelWidth, wb.PixelHeight);
            using (var stream = newWb.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            var newImage = new ImageBrush();
            newImage.ImageSource = newWb;

            return newImage;
        }
    }
}
