using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace GraphicsProject.Classes.Extensions
{
    public static class BrushFilterExtensions
    {
        private static readonly BrushHelper Bh = new BrushHelper();

        public static async void MeanFilter(this Brush brush)
        {
            Bh.Initialize(brush);



            await Bh.UpdateBrush();
        }

        public static async void MedianFilter(this Brush brush)
        {
            Bh.Initialize(brush);

            await Bh.UpdateBrush();
        }

        public static async void SobelFilter(this Brush brush)
        {
            Bh.Initialize(brush);

            await Bh.UpdateBrush();
        }

        public static async void SharpenFilter(this Brush brush)
        {
            Bh.Initialize(brush);

            await Bh.UpdateBrush();
        }

        public static async void GaussianSmoothingFilter(this Brush brush)
        {
            Bh.Initialize(brush);

            await Bh.UpdateBrush();
        }

        public static async void AnyMaskFilter(this Brush brush)
        {
            Bh.Initialize(brush);

            await Bh.UpdateBrush();
        }
    }
}
