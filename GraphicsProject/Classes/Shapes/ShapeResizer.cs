using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    class ShapeResizer
    {
        public void ResizeOnStart(Shape shape, Point startingPoint, Point newPoint)
        {
            var width = newPoint.X - startingPoint.X;
            var height = newPoint.Y - startingPoint.Y;

            if (width >= 0)
            {
                shape.Width = width;
            }

            if (height >= 0)
            {
                shape.Height = height;
            }

            if (width < 0)
            {
                Canvas.SetLeft(shape, newPoint.X);
                shape.Width = startingPoint.X - newPoint.X;
            }

            if (height < 0)
            {
                Canvas.SetTop(shape, newPoint.Y);
                shape.Height = startingPoint.Y - newPoint.Y;
            }
        }

        public void ResizeX(Shape shape, double startX, double newX)
        {
            if (startX > shape.Width / 2)
            {
                if (newX >= 0)
                {
                    shape.Width = newX;
                }
                else
                {
                    var transform = shape.RenderTransform as CompositeTransform;

                    transform.TranslateX += newX;

                    //shape.Width = -newX;
                    //Canvas.SetLeft(shape, startX + newX);
                    //Debug.WriteLine("StartX + newX: " + (startX + newX));
                    Debug.WriteLine("Width " + -newX);
                }
            }
            else
            {
                
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
                Debug.WriteLine("StartY: " + startY + " newY:" + newY);
                Canvas.SetTop(shape, startY + newY);
                shape.Height = -newY;
            }
        }
    }
}
