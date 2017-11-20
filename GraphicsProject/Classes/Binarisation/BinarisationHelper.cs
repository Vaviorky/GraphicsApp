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

            for (var i = 0; i < data.Length; i+=4)
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
    }
}
