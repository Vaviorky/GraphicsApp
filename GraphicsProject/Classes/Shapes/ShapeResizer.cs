using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    class ShapeResizer
    {
        public void Resize(Shape shape, Point startingPoint, Point newPoint)
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
    }
}
