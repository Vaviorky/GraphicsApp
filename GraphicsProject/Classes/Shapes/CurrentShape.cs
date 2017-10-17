using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.System;
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
        public bool IsModyfiying { get; private set; }

        public Shape SelectedShape { get; private set; }

        private CompositeTransform _currentCompositeTransform;
        private readonly ShapeResizer _shapeResizer = new ShapeResizer();
        private readonly ShapePointer _shapePointer = new ShapePointer();
        private Point _startingPoint;
        private Point _endingPoint;

        private ShapeMouseEventType _mouseType;

        public event Action OnShapeSelect = delegate { };
        public event Action OnDeleteShape = delegate { };
        public event Action<Shape> OnShapeParameterChange = delegate { };
        public event Action<Shape> OnShapeModificationComplete = delegate { };

        public void Create(Shape shape, Point startingPosition, Point endingPosition)
        {
            SelectedShape = shape;
            PrepareShape(shape);

            switch (shape)
            {
                case Line line:
                    ShapeDrawer.CreateLine(line, startingPosition, endingPosition);
                    break;
                case Rectangle rectangle:
                    ShapeDrawer.CreateRectangle(rectangle, startingPosition, endingPosition);
                    break;
                case Ellipse ellipse:
                    ShapeDrawer.CreateCircle(ellipse, startingPosition, endingPosition);
                    break;
            }

            _startingPoint = startingPosition;
        }

        public void Resize(Point newPosition)
        {
            if (SelectedShape is Line line)
            {
                line.X2 = newPosition.X;
                line.Y2 = newPosition.Y;
            }
            else
            {
                _shapeResizer.ResizeOnStart(SelectedShape, _startingPoint, newPosition);
                OnShapeParameterChange(SelectedShape);
            }
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

            switch (shape)
            {
                case Line line:
                    _shapePointer.CheckMousePositionForLine(line, mousePos);
                    break;
                case Rectangle rectangle:
                    _shapePointer.CheckMousePosition(shape.Width, shape.Height, mousePos);
                    break;
                case Ellipse ellipse:
                    break;

            }
        }

        private void OnMouseDown(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("On mouse down on object");
            IsModyfiying = true;
            OnShapeSelect();
            SelectedShape = (Shape)sender;
            OnShapeParameterChange(SelectedShape);
            _startingPoint = e.GetCurrentPoint(SelectedShape).Position;
            _currentCompositeTransform = SelectedShape.RenderTransform as CompositeTransform;
            Canvas.SetZIndex(SelectedShape, 2);
        }

        private void OnMouseUp(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Object MouseUp");
            IsModyfiying = false;
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Debug.WriteLine("Shape manipulation started");
            _mouseType = _shapePointer.MouseType;
            this.SelectedShape.Opacity = 0.4;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
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
                case ShapeMouseEventType.MouseOnLineX1:
                    _shapeResizer.ResizeLineX1(SelectedShape as Line, e.Position);
                    break;
                case ShapeMouseEventType.MouseOnLineX2:
                    _shapeResizer.ResizeLineX2(SelectedShape as Line, e.Position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Debug.WriteLine("Manipulation of ellipse finished");
            IsModyfiying = false;
            OnShapeModificationComplete(SelectedShape);
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            this.SelectedShape.Opacity = 1;
        }

        private void PrepareShape(Shape shape)
        {
            shape.RenderTransform = new CompositeTransform();

            shape.PointerPressed += OnMouseDown;
            shape.PointerReleased += OnMouseUp;
            shape.PointerMoved += OnMouseOn;
            shape.PointerExited += (sender, args) => Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);

            shape.ManipulationMode =
                ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            shape.ManipulationStarted += OnManipulationStarted;
            shape.ManipulationDelta += OnManipulationDelta;
            shape.ManipulationCompleted += OnManipulationCompleted;
            shape.RightTapped += (sender, args) => OnDeleteShape();
        }
    }
}
