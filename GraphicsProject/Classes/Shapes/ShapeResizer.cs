using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    class ShapeResizer
    {
        public void ResizeX(Shape shape, double startX, double newX)
        {
            if (newX >= 0)
            {
                shape.Width = newX;
            }
            else
            {
                Canvas.SetLeft(shape, newX);
                shape.Width = Math.Abs(startX - newX);
            }
        }

        public void ResizeY(Shape shape, double startY, double newY)
        {
            if (newY >= 0)
            {
                shape.Height = newY;
            }
            else
            {
                Canvas.SetTop(shape, startY + newY);
                shape.Height = -newY;
            }
        }

        public void ResizeLineX1(Line line, Point newStartPoint)
        {
            line.X1 = newStartPoint.X;
            line.Y1 = newStartPoint.Y;
        }

        public void ResizeLineX2(Line line, Point newEndingPoint)
        {
            line.X2 = newEndingPoint.X;
            line.Y2 = newEndingPoint.Y;
        }

        public void ResizeEllipseX(Shape shape, double startingX, double actualX)
        {
            var diffX = startingX - actualX;

            try
            {
                shape.Width = startingX - diffX;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void ResizeEllipseY(Shape shape, double startingY, double actualY)
        {
            var diffY = startingY - actualY;

            try
            {
                shape.Width = startingY - diffY;
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
