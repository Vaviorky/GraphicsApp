using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using GraphicsProject.Enums;

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

        public void ResetPointer()
        {
            MouseType = ShapeMouseEventType.Dragging;
            Window.Current.CoreWindow.PointerCursor = _arrowCursor;
        }
    }
}
