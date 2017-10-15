using System;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes
{
    public class CurrentShape
    {
        private Shape _currentShape;
        private CompositeTransform _currentCompositeTransform;
        private readonly Random _random;

        private Point _startingPoint;

        private bool isLine = false;

        public bool IsDragging;

        public CurrentShape()
        {
            _random = new Random();
        }

        public event Action OnElementStartModifying = delegate { };

        public void Create(Shape shape, Point position)
        {
            _currentShape = shape;

            if (_currentShape is Line)
            {
                isLine = true;
            }

            AssignEventsToShapeObject(_currentShape);
            _currentShape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255)));

            _currentShape.Width = 1;
            _currentShape.Height = 1;

            _startingPoint = position;

            Debug.WriteLine("Created");

            Canvas.SetLeft(_currentShape, position.X);
            Canvas.SetTop(_currentShape, position.Y);
        }

        public void Modify(Point newPosition)
        {
            var width = newPosition.X - _startingPoint.X;
            var height = newPosition.Y - _startingPoint.Y;

            if (width >= 0)
            {
                _currentShape.Width = width;
            }

            if (height >= 0)
            {
                _currentShape.Height = height;
            }

            if (width < 0)
            {
                Canvas.SetLeft(_currentShape, newPosition.X);
                _currentShape.Width = _startingPoint.X - newPosition.X;
            }

            if (height < 0)
            {
                Canvas.SetTop(_currentShape, newPosition.Y);
                _currentShape.Height = _startingPoint.Y - newPosition.Y;
            }

            if (isLine)
            {
                var line = _currentShape as Line;
                line.X1 = 0;
                line.Y1 = 0;

                Debug.WriteLine(line.Width + " " + line.Height);

                line.X2 = line.Width;
                line.Y2 = line.Height;
                line.StrokeThickness = 5;
                line.Stroke = new SolidColorBrush(Color.FromArgb(0,55,55,55));
            }
        }

        private void OnMouseDown(object sender, PointerRoutedEventArgs e)
        {
            IsDragging = true;
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Debug.WriteLine("Ellipse manipulation started");
            _currentShape = (Shape)sender;
            OnElementStartModifying();
            Canvas.SetZIndex(_currentShape, 2);
            _currentCompositeTransform = _currentShape.RenderTransform as CompositeTransform;
            this._currentShape.Opacity = 0.4;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _currentCompositeTransform.TranslateX += e.Delta.Translation.X;
            _currentCompositeTransform.TranslateY += e.Delta.Translation.Y;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Debug.WriteLine("Manipulation of ellipse finished");
            this._currentShape.Opacity = 1;
        }

        private void OnMouseUp(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Object MouseUp");
            IsDragging = false;
        }

        private void AssignEventsToShapeObject(Shape shape)
        {
            shape.RenderTransform = new CompositeTransform();

            shape.PointerPressed += OnMouseDown;
            shape.PointerReleased += OnMouseUp;

            shape.ManipulationMode =
                ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale;
            shape.ManipulationStarted += OnManipulationStarted;
            shape.ManipulationDelta += OnManipulationDelta;
            shape.ManipulationCompleted += OnManipulationCompleted;
            shape.DoubleTapped += (sender, args) => Debug.WriteLine("DOUBLE TAP!!");
        }
    }
}
