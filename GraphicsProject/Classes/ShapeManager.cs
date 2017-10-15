using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Shapes;
using GraphicsProject.Enums;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace GraphicsProject.Classes
{
    public class ShapeManager
    {
        private readonly Canvas _canvas;
        private readonly CurrentShape _currentShape;
        private readonly List<Shape> _shapes;

        public ShapeManager(Canvas canvas)
        {
            _canvas = canvas;
            _currentShape = new CurrentShape();
            _shapes = new List<Shape>();
            _currentShape.OnElementStartModifying += ResetCanvasIndexOfElements;
        }

        public bool CanModifyShape()
        {
            return !_currentShape.IsDragging;
        }

        public void CreateNewShape(ShapeType shapeType, Point position)
        {
            if (!CanModifyShape()) return;

            var shape = GetCurrentShape(shapeType);
            _shapes.Add(shape);
            _currentShape.Create(shape, position);

            _canvas.Children.Add(shape);
        }

        public void ModifyCreatedShape(Point newPosition)
        {
            _currentShape.Modify(newPosition);
        }

        private Shape GetCurrentShape(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.Line:
                    return new Line();
                case ShapeType.Circle:
                    return new Ellipse();
                case ShapeType.Rectangle:
                    return new Rectangle();
                default:
                    throw new ArgumentOutOfRangeException(nameof(shapeType), shapeType, null);
            }
        }

        private void ResetCanvasIndexOfElements()
        {
            var listIndex = _shapes.Count;
            for (int i = 0; i < listIndex; i++)
            {
                Canvas.SetZIndex(_shapes[i], 0);
            }
        }
    }
}
