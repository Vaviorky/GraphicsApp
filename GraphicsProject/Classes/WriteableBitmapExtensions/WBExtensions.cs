using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphicsProject.Classes.WriteableBitmapExtensions
{
    public static class WbExtensions
    {
        public static void AddByValue(this WriteableBitmap bitmap, decimal value)
        {
            var pixels = GetPixelData(bitmap);

        }

        public static void MultiplyByValue(this WriteableBitmap bitmap, decimal value)
        {

        }

        public static void ChangeBrightness(this WriteableBitmap bitmap, decimal value)
        {

        }

        public static async void MakeGrayscale(this WriteableBitmap bitmap)
        {
            var pixels = GetPixelData(bitmap);
            var length = pixels.Length;

            for (int i = 0; i < length; i += 4)
            {
                var r = pixels[i + 2];
                var g = pixels[i + 1];
                var b = pixels[i];

                var s = (byte)Math.Round((b + g + r) / 3.0, 0, MidpointRounding.AwayFromZero);

                pixels[i + 2] = s;
                pixels[i + 1] = s;
                pixels[i] = s;
            }

            await PixelDataToWriteableBitmap(bitmap, pixels);
        }

        private static byte[] GetPixelData(WriteableBitmap bitmap)
        {
            using (Stream stream = bitmap.PixelBuffer.AsStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        private static async Task PixelDataToWriteableBitmap(WriteableBitmap wb, byte[] pixels)
        {
            using (Stream stream = wb.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(pixels, 0, pixels.Length);
            }
        }
    }
}
