using GraphicsProject.Enums;
using System.Diagnostics;
using System.Globalization;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using GraphicsProject.Classes.Shapes;

namespace GraphicsProject.Views
{
    public sealed partial class Main : Page
    {
        private ShapeType _shapeType;
        private readonly ShapeManager _shapeManager;
        private Point _startingPoint;
        private Point _endingPoint;

        private bool canFinishCreatingShape = true;

        public Main()
        {
            this.InitializeComponent();
            this._shapeType = ShapeType.Circle;
            ButtonRectangleSelect.Background = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
            this._shapeManager = new ShapeManager(DrawingCanvas);
            _shapeManager.OnShapeParameterChange += UpdateTextFields;
        }

        private void UpdateTextFields(Shape shape)
        {
            var cs = shape.TransformToVisual(DrawingCanvas);
            var point = cs.TransformPoint(new Point(shape.Width / 2, shape.Height / 2));
            XPosition.Text = point.X.ToString(CultureInfo.InvariantCulture);
            YPosition.Text = point.Y.ToString(CultureInfo.InvariantCulture);
            Width.Text = shape.Width.ToString(CultureInfo.InvariantCulture);
            Height.Text = shape.Height.ToString(CultureInfo.InvariantCulture);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void Canvas_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //if overall can draw, needs to be added
            if (canFinishCreatingShape)
            {
                _startingPoint = e.GetCurrentPoint(DrawingCanvas).Position;
                canFinishCreatingShape = false;
            }
            else
            {
                _endingPoint = e.GetCurrentPoint(DrawingCanvas).Position;
                _shapeManager.CreateNewShape(_shapeType, _startingPoint, _endingPoint);
                canFinishCreatingShape = true;
            }


            // var point = e.GetCurrentPoint(DrawingCanvas);
            // _shapeManager.CreateNewShape(_shapeType, point.Position);
        }

        private void Canvas_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Canvas mouse up");
        }

        private void XPosition_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Key.ToString().Contains("Number"))
            {
                e.Handled = true;
                return;
            }
            e.Handled = false;
        }

        private void Button_OnLineSelect(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnLineSelect");
            ButtonLineSelect.Background = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
            _shapeType = ShapeType.Line;
        }

        private void Button_OnRectangleSelect(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnLineSelect");

            _shapeType = ShapeType.Rectangle;
        }

        private void Button_OnEllipseSelect(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnLineSelect");
            _shapeType = ShapeType.Circle;
        }
    }
}
