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

        public ShapeManager(Canvas canvas)
        {
            _canvas = canvas;
            this._currentShape = new CurrentShape();
        }

        public void CreateNewShape(ShapeType shapeType, Point position)
        {
            if (_currentShape.IsDragging) return;

            var shape = GetCurrentShape(shapeType);
            _currentShape.Create(shape, position);

            _canvas.Children.Add(shape);
        }

        public void ModifyCreatedShape()
        {
           // _currentShape.
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
    }
}
