using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphicsProject.Classes
{
    internal class BrushHelper
    {
        public byte[] Pixels { get; private set; }
        public int Length { get; private set; }
        public int Width => _bitmap.PixelWidth;
        public int Height => _bitmap.PixelHeight;

        private ImageBrush _brush;
        private WriteableBitmap _bitmap;

        public void Initialize(Brush brush)
        {
            _brush = (ImageBrush)brush;
            _bitmap = (WriteableBitmap)_brush.ImageSource;
            Pixels = _bitmap.PixelBuffer.ToArray();
            Length = Pixels.Length;
        }

        public async Task UpdateBrush()
        {
            await PixelDataToWriteableBitmap();
            _bitmap.Invalidate();
            _brush.ImageSource = _bitmap;
        }

        public async Task UpdateBrush(byte[] pixels)
        {
            Pixels = pixels;
            await UpdateBrush();
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
