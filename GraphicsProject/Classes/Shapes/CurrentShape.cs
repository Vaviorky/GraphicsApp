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
using GraphicsProject.Enums;

namespace GraphicsProject.Classes.Shapes
{
    class CurrentShape
    {
        public bool IsDragging { get; private set; }

        public Shape SelectedShape { get; private set; }

        private CompositeTransform _currentCompositeTransform;
        private readonly Random _random = new Random();
        private readonly LineManager _lineManager = new LineManager();
        private readonly ShapeResizer _shapeResizer = new ShapeResizer();
        private readonly ShapePointer _shapePointer = new ShapePointer();
        private Point _startingPoint;

        private ShapeMouseEventType _mouseType;

        private bool _isLine = false;

        public event Action OnElementStartModifying = delegate { };
        public event Action OnDeleteShape = delegate { };
        public event Action<Shape> OnShapeParameterChange = delegate { };

        public void Create(Shape shape, Point position)
        {
            SelectedShape = shape;
            _startingPoint = position;

            if (SelectedShape is Line)
            {
                _isLine = true;
            }

            AssignEventsToShapeObject(SelectedShape);
            SelectedShape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255)));

            SelectedShape.Width = 0;
            SelectedShape.Height = 0;

            Debug.WriteLine("Created new shape");

            Canvas.SetLeft(SelectedShape, position.X);
            Canvas.SetTop(SelectedShape, position.Y);
        }

        public void Resize(Point newPosition)
        {
            _shapeResizer.ResizeOnStart(SelectedShape, _startingPoint, newPosition);
            OnShapeParameterChange(SelectedShape);
            if (_isLine)
            {
                var line = SelectedShape as Line;
                line.X1 = 0;
                line.Y1 = 0;

                line.X2 = line.Width;
                line.Y2 = line.Height;
                line.StrokeThickness = 5;
                line.Stroke = new SolidColorBrush(Color.FromArgb(0, 55, 55, 55));
            }
        }

        private void OnMouseDown(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("On mouse down on object");
            IsDragging = true;
            OnElementStartModifying();
            SelectedShape = (Shape)sender;
            OnShapeParameterChange(SelectedShape);
            _startingPoint = e.GetCurrentPoint(SelectedShape).Position;
            Debug.WriteLine(_startingPoint);
            _currentCompositeTransform = SelectedShape.RenderTransform as CompositeTransform;
            Canvas.SetZIndex(SelectedShape, 2);
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Debug.WriteLine("Shape manipulation started");
            _mouseType = _shapePointer.MouseType;

            Debug.WriteLine("Mouse type: " + _shapePointer.MouseType);
            this.SelectedShape.Opacity = 0.4;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            //Debug.WriteLine("Mouse sadsada type: " + _shapePointer.MouseType);
            OnShapeParameterChange(SelectedShape);

            switch (_mouseType)
            {
                case ShapeMouseEventType.Dragging:
                    _currentCompositeTransform.TranslateX += e.Delta.Translation.X;
                    _currentCompositeTransform.TranslateY += e.Delta.Translation.Y;
                    break;
                case ShapeMouseEventType.SizeLeftRigt:
                    _shapeResizer.ResizeX(SelectedShape, _startingPoint.X, e.Position.X);
                    break;
                case ShapeMouseEventType.SizeUpDown:
                    _shapeResizer.ResizeY(SelectedShape, _startingPoint.Y, e.Position.Y);
                    break;
                case ShapeMouseEventType.SizeLeftUpRightDown:
                    break;
                case ShapeMouseEventType.SizeLeftDownRightUp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Debug.WriteLine("Manipulation of ellipse finished");
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            this.SelectedShape.Opacity = 1;
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

            if (!shiftIsPressed)
            {
                _shapePointer.ResetPointer();
                return;
            }

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
            // shape.PointerExited += (sender, args) => _shapePointer.ResetPointer();

            shape.ManipulationMode =
                ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale;
            shape.ManipulationStarted += OnManipulationStarted;
            shape.ManipulationDelta += OnManipulationDelta;
            shape.ManipulationCompleted += OnManipulationCompleted;
            shape.RightTapped += (sender, args) => OnDeleteShape();
            shape.DoubleTapped += (sender, args) => Debug.WriteLine("DOUBLE TAP!!");
        }

        public void ChangeColor(Color color)
        {
            if (SelectedShape != null)
            {
                SelectedShape.Fill = new SolidColorBrush(color);
            }
        }
    }
}
