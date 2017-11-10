using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

public static class BrushExtensions
{
    private static ImageBrush _brush;
    private static WriteableBitmap _bitmap;
    private static byte[] _pixels;
    private static int _length;

    public static async void MakeGrayscale(this Brush brush)
    {
        Initialize(brush);

        for (int i = 0; i < _length; i += 4)
        {
            var r = _pixels[i + 2];
            var g = _pixels[i + 1];
            var b = _pixels[i];

            var s = (byte)Math.Round((r + g + b) / 3.0, 0, MidpointRounding.AwayFromZero);

            _pixels[i + 2] = s;
            _pixels[i + 1] = s;
            _pixels[i] = s;
        }

        await UpdateBrush();
    }

    public static async void AddPixelByValue(this Brush brush, decimal value)
    {
        Initialize(brush);

        for (int i = 0; i < _length; i += 4)
        {
            var r = _pixels[i + 2] + value;
            var g = _pixels[i + 1] + value;
            var b = _pixels[i] + value;

            if (r > 255)
            {
                r = 255;
            }
            else if (r < 0)
            {
                r = 0;
            }

            if (g > 255)
            {
                g = 255;
            }
            else if (g < 0)
            {
                g = 0;
            }

            if (b > 255)
            {
                b = 255;
            }
            else if (b < 0)
            {
                b = 0;
            }

            _pixels[i + 2] = (byte)r;
            _pixels[i + 1] = (byte)g;
            _pixels[i] = (byte)b;
        }

        await UpdateBrush();
    }

    public static async void MultiplyPixelByValue(this Brush brush, decimal value)
    {
        Initialize(brush);
        await UpdateBrush();
    }

    private static byte[] GetPixelData()
    {
        using (Stream stream = _bitmap.PixelBuffer.AsStream())
        using (MemoryStream memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
    private static async Task PixelDataToWriteableBitmap()
    {
        using (Stream stream = _bitmap.PixelBuffer.AsStream())
        {
            await stream.WriteAsync(_pixels, 0, _length;
        }
    }

    private static void Initialize(Brush brush)
    {
        _brush = (ImageBrush)brush;
        var bitmapSource = (BitmapSource)_brush.ImageSource;
        _bitmap = (WriteableBitmap)bitmapSource;
        _pixels = GetPixelData();
        _length = _pixels.Length;
    }

    private static async Task UpdateBrush()
    {
        await PixelDataToWriteableBitmap();
        _brush.ImageSource = _bitmap;
    }
}
