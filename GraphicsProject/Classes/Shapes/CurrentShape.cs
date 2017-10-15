using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    class CurrentShape
    {
        private Shape _currentShape;
        private CompositeTransform _currentCompositeTransform;
        private readonly Random _random = new Random();
        private readonly LineManager _lineManager = new LineManager();
        private readonly ShapeResizer _shapeResizer = new ShapeResizer();
        private readonly ShapePointer _shapePointer = new ShapePointer();

        private Point _startingPoint;

        private bool isLine = false;
        public bool IsDragging { get; private set; }

        public event Action OnElementStartModifying = delegate { };

        public void Create(Shape shape, Point position)
        {
            _currentShape = shape;
            _startingPoint = position;

            if (_currentShape is Line)
            {
                isLine = true;
            }

            AssignEventsToShapeObject(_currentShape);
            _currentShape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255)));

            _currentShape.Width = 0;
            _currentShape.Height = 0;

            Debug.WriteLine("Created");

            Canvas.SetLeft(_currentShape, position.X);
            Canvas.SetTop(_currentShape, position.Y);
        }

        public void Resize(Point newPosition)
        {
            _shapeResizer.Resize(_currentShape, _startingPoint, newPosition);

            if (isLine)
            {
                var line = _currentShape as Line;
                line.X1 = 0;
                line.Y1 = 0;

                Debug.WriteLine(line.Width + " " + line.Height);

                line.X2 = line.Width;
                line.Y2 = line.Height;
                line.StrokeThickness = 5;
                line.Stroke = new SolidColorBrush(Color.FromArgb(0, 55, 55, 55));
            }
        }

        private void OnMouseDown(object sender, PointerRoutedEventArgs e)
        {
            IsDragging = true;
            OnElementStartModifying();
            _currentShape = (Shape)sender;
            _currentCompositeTransform = _currentShape.RenderTransform as CompositeTransform;
            Canvas.SetZIndex(_currentShape, 2);
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Debug.WriteLine("Ellipse manipulation started");
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeAll, 0);
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
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            this._currentShape.Opacity = 1;
        }

        private void OnMouseUp(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Object MouseUp");
            IsDragging = false;
        }

        private void OnMouseOn(object sender, PointerRoutedEventArgs e)
        {
            var shiftIsPressed = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift)
                .HasFlag(CoreVirtualKeyStates.Down);

            if (!shiftIsPressed) return;

            var shape = (Shape)sender;
            var mousePos = e.GetCurrentPoint(shape).Position;

            _shapePointer.CheckMousePosition(shape.Width, shape.Height, mousePos);
        }

        private void AssignEventsToShapeObject(Shape shape)
        {
            shape.RenderTransform = new CompositeTransform();

            shape.PointerPressed += OnMouseDown;
            shape.PointerReleased += OnMouseUp;
            shape.PointerMoved += OnMouseOn;
            shape.PointerExited += (sender, args) => _shapePointer.ResetPointer();

            shape.ManipulationMode =
                ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale;
            shape.ManipulationStarted += OnManipulationStarted;
            shape.ManipulationDelta += OnManipulationDelta;
            shape.ManipulationCompleted += OnManipulationCompleted;
            shape.DoubleTapped += (sender, args) => Debug.WriteLine("DOUBLE TAP!!");
        }

        public void ChangeColor(Color color)
        {
            _currentShape.Fill = new SolidColorBrush(color);
        }
    }
}
