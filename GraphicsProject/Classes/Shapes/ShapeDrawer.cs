using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    internal class ShapeDrawer
    {
        public static void InitializeRectangle(Rectangle rectangle, Point startingPosition, Point endingPosition)
        {
            var x = startingPosition.X < endingPosition.X ? startingPosition.X : endingPosition.X;
            var y = startingPosition.Y < endingPosition.Y ? startingPosition.Y : endingPosition.Y;

            rectangle.Width = Math.Abs(endingPosition.X - startingPosition.X);
            rectangle.Height = Math.Abs(endingPosition.Y - startingPosition.Y);

            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);
        }

        public static void InitializeCircle(Ellipse ellipse, Point startingPosition, Point endingPosition)
        {
            var x = startingPosition.X < endingPosition.X ? startingPosition.X : endingPosition.X;
            var y = startingPosition.Y < endingPosition.Y ? startingPosition.Y : endingPosition.Y;

            var width = Math.Abs(endingPosition.X - startingPosition.X);
            var height = Math.Abs(endingPosition.Y - startingPosition.Y);

            ellipse.Width = width > height ? width : height;
            ellipse.Height = width > height ? width : height;

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
        }

        public static void InitializeLine(Line line, Point startingPosition, Point endingPosition)
        {
            var width = startingPosition.X > endingPosition.X ? startingPosition.X : endingPosition.X;
            var height = startingPosition.Y > endingPosition.Y ? startingPosition.Y : endingPosition.Y;

            Canvas.SetLeft(line, 0);
            Canvas.SetTop(line, 0);

            line.X1 = startingPosition.X;
            line.Y1 = startingPosition.Y;

            line.X2 = endingPosition.X;
            line.Y2 = endingPosition.Y;

            line.Width = width;
            line.Height = height;
        }

        
    }
}
