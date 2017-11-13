using Windows.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Text;


namespace GraphicsProject.Classes.ImageProcessing
{
    public static class BrushFilterExtensions
    {
        private static readonly BrushHelper Bh = new BrushHelper();

        private static byte[] _origin;
        private static byte[] _temp;

        private static int _width;

        public static async void MeanFilter(this Brush brush)
        {
            Bh.Initialize(brush);
            InitArrays();
            _width = Bh.Width * 4;
            var height = Bh.Height;

            for (int x = 1; x < height - 1; x++)
            {
                for (int j = 4; j < _width - 5; j += 4)
                {
                    int i = j + _width * x;
                    GoThroughMask(Mask.Mean, i, _width, 9);
                }
            }

            await Bh.UpdateBrush(_temp);
        }

        public static async void MedianFilter(this Brush brush)
        {
            Bh.Initialize(brush);
            InitArrays();
            _width = Bh.Width * 4;
            var height = Bh.Height;

            for (int x = 1; x < height - 1; x++)
            {
                for (int j = 4; j < _width - 5; j += 4)
                {
                    int i = j + _width * x;

                    var r = GetMedian(i, 0);
                    var g = GetMedian(i, 1);
                    var b = GetMedian(i, 2);

                    _temp[i + 2] = (byte)r;
                    _temp[i + 1] = (byte)g;
                    _temp[i] = (byte)b;
                }
            }

            await Bh.UpdateBrush(_temp);
        }

        private static int GetMedian(int x, int offset)
        {
            var mask = new int[9];

            mask[0] = _origin[x - (2 + offset)];
            mask[1] = _origin[x + (2 - offset)];
            mask[2] = _origin[x + (6 - offset)];
            mask[3] = _origin[x - (2 + offset) - _width];
            mask[4] = _origin[x + (2 - offset) - _width];
            mask[5] = _origin[x + (6 - offset) - _width];
            mask[6] = _origin[x - (2 + offset) + _width];
            mask[7] = _origin[x + (2 - offset) + _width];
            mask[8] = _origin[x + (6 - offset) + _width];

            for (int i = 0; i < mask.Length - 1; i++)
            {
                for (int j = 0; j < mask.Length - 1; j++)
                {
                    if (mask[j] <= mask[j + 1]) continue;

                    var buf = mask[j];
                    mask[j] = mask[j + 1];
                    mask[j + 1] = buf;
                }
            }
            return mask[1];
        }

        public static async void SobelFilter(this Brush brush)
        {
            Bh.Initialize(brush);
            InitArrays();
            var width = Bh.Width * 4;
            var height = Bh.Height;

            for (int x = 1; x < height - 1; x++)
            {
                for (int j = 4; j < width - 5; j += 4)
                {
                    int i = j + width * x;

                    GoThroughMask(Mask.Sobel, i, width, 1);
                }
            }

            await Bh.UpdateBrush(_temp);
        }

        public static async void SharpFilter(this Brush brush)
        {
            Bh.Initialize(brush);
            InitArrays();
            var width = Bh.Width * 4;
            var height = Bh.Height;

            for (int x = 1; x < height - 1; x++)
            {
                for (int j = 4; j < width - 5; j += 4)
                {
                    int i = j + width * x;
                    GoThroughMask(Mask.Sharp, i, width, 1);
                }
            }

            await Bh.UpdateBrush(_temp);
        }

        public static async void GaussFilter(this Brush brush)
        {
            Bh.Initialize(brush);
            InitArrays();
            var width = Bh.Width * 4;
            var height = Bh.Height;

            for (int x = 1; x < height - 1; x++)
            {
                for (int j = 4; j < width - 5; j += 4)
                {
                    int i = j + width * x;
                    GoThroughMask(Mask.Gauss, i, width, 16);
                }
            }

            await Bh.UpdateBrush(_temp);
        }

        private static void GoThroughMask(int[,] mask, int i, int width, int div)
        {
            var r = (mask[0, 0] * _origin[i - 2] + mask[0, 1] * _origin[i + 2] + mask[0, 2] * _origin[i + 6] +
                    mask[1, 0] * _origin[i - 2 - width] + mask[1, 1] * _origin[i + 2 - width] + mask[1, 2] * _origin[i + 6 - width] +
                    mask[2, 0] * _origin[i - 2 + width] + mask[2, 1] * _origin[i + 2 + width] + mask[2, 2] * _origin[i + 6 + width]) / div;

            var g = (mask[0, 0] * _origin[i - 3] + mask[0, 1] * _origin[i + 1] + mask[0, 2] * _origin[i + 5] +
                     mask[1, 0] * _origin[i - 3 - width] + mask[1, 1] * _origin[i + 1 - width] + mask[1, 2] * _origin[i + 5 - width] +
                     mask[2, 0] * _origin[i - 3 + width] + mask[2, 1] * _origin[i + 1 + width] + mask[2, 2] * _origin[i + 5 + width]) / div;

            var b = (mask[0, 0] * _origin[i - 4] + mask[0, 1] * _origin[i] + mask[0, 2] * _origin[i + 4] +
                    mask[1, 0] * _origin[i - 4 - width] + mask[1, 1] * _origin[i - width] + mask[1, 2] * _origin[i + 4 - width] +
                    mask[2, 0] * _origin[i - 4 + width] + mask[2, 1] * _origin[i + width] + mask[2, 2] * _origin[i + 4 + width]) / div;

            _temp[i + 2] = (byte)(r > 0 ? r : 0);
            _temp[i + 1] = (byte)(g > 0 ? g : 0);
            _temp[i] = (byte)(b > 0 ? b : 0);
        }

        private static void InitArrays()
        {
            _origin = new byte[Bh.Length];
            _temp = new byte[Bh.Length];
            Bh.Pixels.CopyTo(_origin, 0);
            Bh.Pixels.CopyTo(_temp, 0);
        }
    }
}
