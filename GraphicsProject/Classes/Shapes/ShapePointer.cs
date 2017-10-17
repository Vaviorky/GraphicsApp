using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GraphicsProject.Enums;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    class ShapePointer
    {
        private readonly CoreCursor _arrowCursor = new CoreCursor(CoreCursorType.Arrow, 0);
        private readonly CoreCursor _sizeWestEastCursor = new CoreCursor(CoreCursorType.SizeWestEast, 0);
        private readonly CoreCursor _sizeNorthSouthCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 0);

        public ShapeMouseEventType MouseType { get; private set; }

        public void CheckMousePosition(double width, double height, Point mousePos)
        {
            if (mousePos.X <= 10 || mousePos.X > width - 10)
            {
                MouseType = ShapeMouseEventType.SizeLeftRigt;
                Window.Current.CoreWindow.PointerCursor = _sizeWestEastCursor;
            }
            else if (mousePos.Y <= 10 || mousePos.Y > height - 10)
            {
                MouseType = ShapeMouseEventType.SizeUpDown;
                Window.Current.CoreWindow.PointerCursor = _sizeNorthSouthCursor;
            }
            else
            {
                ResetPointer();
            }
        }


        public void CheckMousePositionForLine(Line line, Point mousePos)
        {
            if (Math.Abs(mousePos.X - line.X1) < 10 || Math.Abs(mousePos.Y - line.Y1) < 10)
            {
                MouseType = ShapeMouseEventType.MouseOnLineX1;
                Window.Current.CoreWindow.PointerCursor = _sizeWestEastCursor;
            }

            if (Math.Abs(mousePos.X - line.X2) < 10 || Math.Abs(mousePos.Y - line.Y2) < 10)
            {
                MouseType = ShapeMouseEventType.MouseOnLineX2;
                Window.Current.CoreWindow.PointerCursor = _sizeWestEastCursor;
            }
        }

        public void ResetPointer()
        {
            MouseType = ShapeMouseEventType.Dragging;
            Window.Current.CoreWindow.PointerCursor = _arrowCursor;
        }
    }
}
