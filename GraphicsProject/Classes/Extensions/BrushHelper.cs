using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphicsProject.Classes.Extensions
{
    internal class BrushHelper
    {
        public byte[] Pixels { get; private set; }
        public int Length { get; private set; }

        private ImageBrush _brush;
        private WriteableBitmap _bitmap;

        public void Initialize(Brush brush)
        {
            _brush = (ImageBrush)brush;
            var source = (BitmapSource)_brush.ImageSource;
            _bitmap = (WriteableBitmap)source;
            Pixels = GetPixelData();
            Length = Pixels.Length;
        }

        public async Task UpdateBrush()
        {
            await PixelDataToWriteableBitmap();
            _bitmap.Invalidate();
            _brush.ImageSource = _bitmap;
        }

        private byte[] GetPixelData()
        {
            using (Stream stream = _bitmap.PixelBuffer.AsStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private async Task PixelDataToWriteableBitmap()
        {
            using (Stream stream = _bitmap.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(Pixels, 0, Length);
            }
        }
    }
}
