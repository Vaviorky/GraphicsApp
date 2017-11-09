using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GraphicsProject.Classes.WriteableBitmapExtensions;

namespace GraphicsProject.Classes.ImgTransformation
{
    class ImageTransformation
    {
        private readonly Canvas _canvas;

        public ImageTransformation(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void MakeGrayscale(WriteableBitmap bitmap)
        {
            bitmap.MakeGrayscale();
            UpdateImage(bitmap);

        }

        private void UpdateImage(WriteableBitmap bitmap)
        {
            var brush = new ImageBrush() {ImageSource = bitmap};
            _canvas.Background = brush;
        }

    }
}
