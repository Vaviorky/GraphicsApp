using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GraphicsProject.Classes.ColorPicker
{
    internal class ColorPicker
    {
        private Image _picker;
        private byte[] _bytes;
        private WriteableBitmap _wb;
        private const int Size = 256;

        public ColorPicker(Image picker)
        {
            _picker = picker;
            _bytes = new byte[Size * Size * 4];
            SetColor(255, 0, 0);
            _wb = new WriteableBitmap(Size, Size);
            InitializePicker();
        }

        private async void InitializePicker()
        {

            _picker.Source = await PixelBufferToWriteableBitmap();
            //bitmap.
        }

        private async Task<WriteableBitmap> PixelBufferToWriteableBitmap()
        {
            using (Stream stream = _wb.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(_bytes, 0, _bytes.Length);
            }
            return _wb;
        }

        private void SetColor(int r, int g, int b)
        {
            int length = _bytes.Length;

            for (int i = 0, j = 0; i < length; i += 4, j += 3)
            {
                //_bytes[i] = (byte)(numbers[j + 2] * 255);
                //_bytes[i + 1] = (byte)(numbers[j + 1] * 255);
                //_bytes[i + 2] = (byte)(numbers[j] * 255);
                _bytes[i + 3] = 255;
            }

        }

    }
}
