using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using GraphicsProject.Enums;

namespace GraphicsProject.Classes.Shapes
{
    class ShapeManager
    {
        public event Action<Shape> OnShapeParameterChange = delegate { };

        private readonly Canvas _canvas;
        private readonly CurrentShape _currentShape;
        private readonly List<Shape> _shapes;

        public ShapeManager(Canvas canvas)
        {
            _canvas = canvas;
            _currentShape = new CurrentShape();
            _shapes = new List<Shape>();
            _currentShape.OnElementStartModifying += ResetCanvasIndexOfElements;
            _currentShape.OnDeleteShape += RemoveShapeFromCanvas;
            _currentShape.OnShapeParameterChange += SendChangingParametersInformation;
        }

        private void SendChangingParametersInformation(Shape shape)
        {
            OnShapeParameterChange(shape);
        }

        public bool CanModifyShape()
        {
            return !_currentShape.IsDragging;
        }

        public void CreateNewShape(ShapeType shapeType, Point position)
        {
            if (!CanModifyShape()) return;

            ResetCanvasIndexOfElements();

            var shape = GetCurrentShape(shapeType);
            _shapes.Add(shape);
            _currentShape.Create(shape, position);

            _canvas.Children.Add(shape);
        }

        public void ModifyCreatedShape(Point newPosition)
        {
            _currentShape.Resize(newPosition);
        }

        public void ChangeColorOfSelectedShape(Color color)
        {
            _currentShape.ChangeColor(color);
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

        private void RemoveShapeFromCanvas()
        {
            _shapes.Remove(_currentShape.SelectedShape);
            _canvas.Children.Remove(_currentShape.SelectedShape);
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
