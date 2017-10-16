using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    class LineManager
    {
        public void ResizeOnStart(Line line, Point startingPosition, Point newPosition)
        {
            Debug.WriteLine(newPosition.X - startingPosition.X);

            if (newPosition.X - startingPosition.X > 0)
            {
                line.Width = newPosition.X;
                line.Height = newPosition.Y;
                line.X2 = newPosition.X - startingPosition.X;
                line.Y2 = newPosition.Y - startingPosition.Y;
            }
        }
    }
}
