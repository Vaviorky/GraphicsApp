using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    class LineManager
    {
        public void ResizeOnStart(Line line, Point startingPosition, Point newPosition)
        {
            line.X2 = newPosition.X - startingPosition.X;
            line.Y2 = newPosition.Y - startingPosition.Y;
        }
    }
}
