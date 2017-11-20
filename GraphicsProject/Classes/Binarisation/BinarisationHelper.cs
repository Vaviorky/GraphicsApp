using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphicsProject.Classes.Binarisation
{
    public static class BinarisationHelper
    {
        public static async Task<ImageBrush> ManualBinarisationPreview(ImageBrush image, int threshold)
        {
            var wb = (WriteableBitmap)image.ImageSource;
            var data = wb.PixelBuffer.ToArray();

            for (var i = 0; i < data.Length; i += 4)
            {
                var oldPixelValue = data[i];
                var value = oldPixelValue <= threshold ? 0 : 255;

                data[i + 2] = (byte)value;
                data[i + 1] = (byte)value;
                data[i] = (byte)value;
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

        public static int[] GetBlackPercentLut(int percent, int size, int[] histogram)
        {
            int[] lut = new int[256];
            int sum = 0;
            int threshold = (int)(percent * size * 0.01);
            for (int i = 0; i < lut.Length; i++)
            {
                sum += histogram[i];
                if (sum >= threshold)
                {
                    lut[i] = 255;
                }
            }
            return lut;
        }

        public static int GetOtsuThreshold(ImageBrush image)
        {
            var wb = (WriteableBitmap)image.ImageSource;
            var data = wb.PixelBuffer.ToArray();
            var pixels = GetPixelValues(data);

            var bgWeight = new double[256];
            var bgMean = new double[256];
            var fgWeight = new double[256];
            var fgMean = new double[256];

            var max = wb.PixelWidth * wb.PixelHeight;
            for (var i = 0; i < 256; i++)
            {
                bgWeight[i] = GetWeight(pixels, max, 0, i);
                bgMean[i] = GetAverage(pixels, 0, i);

                fgWeight[i] = GetWeight(pixels, max, i + 1, 256);
                fgMean[i] = GetAverage(pixels, i + 1, 256);
            }

            double maximum = 0;
            int position = 0;

            for (int j = 0; j < 256; j++)
            {
                var result = bgWeight[j] * fgWeight[j] * (bgMean[j] - fgMean[j]) * (bgMean[j] - fgMean[j]);
                if (result > maximum)
                {
                    maximum = result;
                    position = j;
                }
            }
            return position;
        }

        private static int[] GetPixelValues(byte[] data)
        {
            var tab = new int[256];
            for (int i = 0; i < data.Length; i+=4)
            {
                var value = data[i + 2];
                tab[value]++;
            }
            return tab;
        }

        private static double GetWeight(int[] pixelsValues, int maxPixels, int start, int end)
        {
            double suma = 0;
            for (var i = start; i < end; i++)
            {
                suma += pixelsValues[i];
            }
            return suma / maxPixels;
        }

        private static double GetAverage(int[] pixelsValues, int start, int end)
        {
            double max = 0;
            double suma = 0;
            for (var i = start; i < end; i++)
            {
                suma += pixelsValues[i] * i;
                max += pixelsValues[i];
            }
            return suma / max;
        }
    }
}
