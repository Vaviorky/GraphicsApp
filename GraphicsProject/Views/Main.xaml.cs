using System;
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

        private bool _canFinishCreatingShape = true;

        public Main()
        {
            this.InitializeComponent();
            this._shapeType = ShapeType.Line;
            this._shapeManager = new ShapeManager(DrawingCanvas);
            _shapeManager.OnShapeParameterChange += UpdateTextFields;
        }

        private void UpdateTextFields(Shape shape)
        {
            var cs = shape.TransformToVisual(DrawingCanvas);
            var point = cs.TransformPoint(new Point(shape.Width / 2, shape.Height / 2));
            XPosition.Text = point.X.ToString(CultureInfo.InvariantCulture);
            YPosition.Text = point.Y.ToString(CultureInfo.InvariantCulture);


        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        #region CanvasEvents
        
        private void Canvas_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {

            //if overall can draw, needs to be added
            if (_canFinishCreatingShape)
            {
                _startingPoint = e.GetCurrentPoint(DrawingCanvas).Position;
                _canFinishCreatingShape = false;
            }
            else
            {
                _endingPoint = e.GetCurrentPoint(DrawingCanvas).Position;
                _shapeManager.CreateNewShape(_shapeType, _startingPoint, _endingPoint);
                _canFinishCreatingShape = true;
            }
        }

        #endregion

        #region ShapeButtonSelection

        private void Button_OnLineSelect(object sender, RoutedEventArgs e)
        {
            _shapeType = ShapeType.Line;
            SelectButton(_shapeType);
        }

        private void Button_OnRectangleSelect(object sender, RoutedEventArgs e)
        {
            _shapeType = ShapeType.Rectangle;
            SelectButton(_shapeType);
        }

        private void Button_OnEllipseSelect(object sender, RoutedEventArgs e)
        {
            _shapeType = ShapeType.Circle;
            SelectButton(_shapeType);
        }

        private void SelectButton(ShapeType type)
        {
            ButtonLineSelect.Background = new SolidColorBrush(Colors.ForestGreen);
            ButtonEllipseSelect.Background = new SolidColorBrush(Colors.ForestGreen);
            ButtonRectangleSelect.Background = new SolidColorBrush(Colors.ForestGreen);

            LineProperties.Visibility = Visibility.Collapsed;
            CircleProperties.Visibility = Visibility.Collapsed;
            RectangleProperties.Visibility = Visibility.Collapsed;

            switch (type)
            {
                case ShapeType.Line:
                    ButtonLineSelect.Background = new SolidColorBrush(Colors.LawnGreen);
                    LineProperties.Visibility = Visibility.Visible;
                    break;
                case ShapeType.Circle:
                    ButtonEllipseSelect.Background = new SolidColorBrush(Colors.LawnGreen);
                    CircleProperties.Visibility = Visibility.Visible;
                    break;
                case ShapeType.Rectangle:
                    ButtonRectangleSelect.Background = new SolidColorBrush(Colors.LawnGreen);
                    RectangleProperties.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new NotImplementedException("You need to check for this shape type: " + type);
            }
        }

        #endregion

        #region ShapePropertiesChanged

        private void Shape_PositionXChanged(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Shape_PositionYChanged(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Line_X1Changed(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Line_Y1Changed(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Line_X2Changed(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Line_Y2Changed(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Rectangle_WidthChanged(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Rectangle_HeightChanged(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Circle_RadiusChanged(object sender, KeyRoutedEventArgs e)
        {

        }

        #endregion



    }
}
