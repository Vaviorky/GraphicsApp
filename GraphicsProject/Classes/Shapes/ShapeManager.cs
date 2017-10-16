using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using GraphicsProject.Enums;
using System.Diagnostics;
using Windows.UI.Xaml.Media;

namespace GraphicsProject.Classes.Shapes
{
    internal class ShapeManager
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

        public void CreateNewShape(ShapeType shapeType, Point startingPosition, Point endingPosition)
        {
            if (!CanModifyShape()) return;

            ResetCanvasIndexOfElements();

            var shape = GetCurrentShape(shapeType);
            _shapes.Add(shape);
            _currentShape.Create(shape, startingPosition, endingPosition);

            _canvas.Children.Add(shape);
        }

        public void ModifyCreatedShape(Point newPosition)
        {
            _currentShape.Resize(newPosition);
        }

        private Shape GetCurrentShape(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.Line:
                    var line = new Line
                    {
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 3
                    };
                    return line;

                case ShapeType.Circle:
                    var ellipse = new Ellipse
                    {
                        Fill = new SolidColorBrush(Colors.DodgerBlue)
                    };
                    return ellipse;

                case ShapeType.Rectangle:
                    var rectangle = new Rectangle
                    {
                        Fill = new SolidColorBrush(Colors.SeaGreen)
                    };
                    return rectangle;
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
            var listCount = _shapes.Count;
            for (int i = 0; i < listCount; i++)
            {
                Canvas.SetZIndex(_shapes[i], 0);
            }
        }
    }
}
