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
        public bool HasShapeBeenDeleted { get; set; }

        public event Action<Shape> OnShapeParameterChange = delegate { };
        public event Action<Shape> OnShapeSelection = delegate { };
        public event Action OnShapeModificationComplete = delegate { };

        public bool ModifyingShape => _currentShape.IsModyfiying;

        private readonly Canvas _canvas;
        private readonly CurrentShape _currentShape;
        private readonly List<Shape> _shapes;

        public ShapeManager(Canvas canvas)
        {
            _canvas = canvas;
            _currentShape = new CurrentShape();
            _shapes = new List<Shape>();
            _currentShape.OnShapeSelect += ResetCanvasIndexOfElements;
            _currentShape.OnShapeSelect += SelectShape;
            _currentShape.OnDeleteShape += RemoveShapeFromCanvas;
            _currentShape.OnShapeParameterChange += SendChangingParametersInformation;
            _currentShape.OnShapeModificationComplete += ShapeModifitationComplete;
        }

        private void SelectShape(Shape shape)
        {
            OnShapeSelection(shape);
        }

        private void SendChangingParametersInformation(Shape shape)
        {
            OnShapeParameterChange(shape);
        }

        private void ShapeModifitationComplete()
        {
            OnShapeModificationComplete();
        }

        public void CreateNewShape(ShapeType shapeType, Point startingPosition, Point endingPosition)
        {
            if (ModifyingShape) return;


            var shape = GetCurrentShape(shapeType);
            ResetCanvasIndexOfElements(shape);
            _shapes.Add(shape);
            _currentShape.Create(shape, startingPosition, endingPosition);

            OnShapeParameterChange(shape);

            _canvas.Children.Add(shape);
        }

        #region Modifying Shapes
        public void SetWidthAndHeight(double width, double height)
        {
            _currentShape.SelectedShape.Width = width;
            _currentShape.SelectedShape.Height = height;
        }

        public void SetPosition(double x, double y)
        {
            Canvas.SetLeft(_currentShape.SelectedShape, x);
            Canvas.SetTop(_currentShape.SelectedShape, y);
        }

        public void SetCircleRadius(double radius)
        {
            var size = 2 * radius;
            var ellipse = _currentShape.SelectedShape as Ellipse;

            ellipse.Width = size;
            ellipse.Height = size;

            //var xPosition = Canvas.GetLeft(ellipse);
            //var yPosition = Canvas.GetTop(ellipse);

            //Canvas.SetLeft(ellipse, xPosition - radius);
            //Canvas.SetTop(ellipse, yPosition - radius);
        }

        #endregion

        public void SetLineParameters(double x1, double y1, double x2, double y2)
        {
            if (_currentShape.SelectedShape is Line line)
            {
                line.X1 = x1;
                line.Y1 = y1;
                line.X2 = x2;
                line.Y2 = y2;
            }
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
            HasShapeBeenDeleted = true;
        }

        private void ResetCanvasIndexOfElements(Shape shape)
        {
            var listCount = _shapes.Count;
            for (int i = 0; i < listCount; i++)
            {
                Canvas.SetZIndex(_shapes[i], 0);
            }
        }
    }
}
