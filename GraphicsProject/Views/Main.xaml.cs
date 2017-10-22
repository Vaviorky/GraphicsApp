using System;
using GraphicsProject.Enums;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using GraphicsProject.Classes.ImageManagement;
using GraphicsProject.Classes.Shapes;

namespace GraphicsProject.Views
{
    public sealed partial class Main : Page
    {
        private ShapeType _shapeType;
        private ShapeManager _shapeManager;

        private ImageManager _imageManager;

        private Point _startingPoint;
        private Point _endingPoint;
        private bool _canStartCreatingShape = true;
        private bool _hasShapeBeenSelected = false;
        private bool _mouseDownOnCanvas;

        public Main()
        {
            this.InitializeComponent();
            _imageManager = new ImageManager(DrawingCanvas);
            InitializeShapeManager();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        #region Shapes

        private void UpdateTextFields(Shape shape)
        {
            var cs = shape.TransformToVisual(DrawingCanvas);

            Point point;

            if (shape is Line line)
            {
                point = cs.TransformPoint(new Point((line.X1 + line.X2) / 2, (line.Y1 + line.Y2) / 2));
            }
            else
            {
                point = cs.TransformPoint(new Point(shape.Width / 2, shape.Height / 2));
            }

            XPosition.Text = Math.Round(point.X, 2).ToString();
            YPosition.Text = Math.Round(point.Y, 2).ToString();

            UpdateShapeSpecialInformation(shape);
        }

        private void OnShapeSelection(Shape shape)
        {
            switch (shape)
            {
                case Ellipse ellipse:
                    _shapeType = ShapeType.Circle;
                    break;
                case Line line:
                    _shapeType = ShapeType.Line;
                    break;
                case Rectangle rectangle:
                    _shapeType = ShapeType.Rectangle;
                    break;
            }
            SelectButton(_shapeType);
            UpdateTextFields(shape);

            _hasShapeBeenSelected = true;
        }

        private void ResetDrawing()
        {
            _canStartCreatingShape = true;
        }

        #region CanvasEvents

        private void InitializeShapeManager()
        {
            this._shapeManager = new ShapeManager(DrawingCanvas);
            _shapeManager.OnShapeSelection += OnShapeSelection;
            _shapeManager.OnShapeParameterChange += UpdateTextFields;
            _shapeManager.OnShapeModificationComplete += ResetDrawing;
            this._shapeType = ShapeType.Line;
        }

        private void Canvas_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Canvas on pointer pressed");
            _mouseDownOnCanvas = true;

            if (_hasShapeBeenSelected)
            {
                _hasShapeBeenSelected = false;
                _canStartCreatingShape = true;
                return;
            }

            if (_shapeManager.HasShapeBeenDeleted)
            {
                _shapeManager.HasShapeBeenDeleted = false;
                _canStartCreatingShape = true;
            }

            if (_canStartCreatingShape)
            {
                _startingPoint = e.GetCurrentPoint(DrawingCanvas).Position;
                _canStartCreatingShape = false;
            }
            else
            {
                _endingPoint = e.GetCurrentPoint(DrawingCanvas).Position;
                _shapeManager.CreateNewShape(_shapeType, _startingPoint, _endingPoint);
                _canStartCreatingShape = true;
            }
        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _mouseDownOnCanvas = false;
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
            _canStartCreatingShape = true;
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

        private void XPosition_TextChanged(object sender, TextChangedEventArgs e)
        {
            Shape_SetPosition();
        }

        private void YPosition_TextChanged(object sender, TextChangedEventArgs e)
        {
            Shape_SetPosition();
        }

        private void Line_X1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Line_SetParameters();
        }

        private void Line_Y1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Line_SetParameters();
        }

        private void Line_X2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Line_SetParameters();
        }

        private void Line_Y2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Line_SetParameters();
        }

        private void Rectangle_Width_TextChanged(object sender, TextChangedEventArgs e)
        {
            Rectangle_SetWidthAndHeight();
        }

        private void Rectangle_Height_TextChanged(object sender, TextChangedEventArgs e)
        {
            Rectangle_SetWidthAndHeight();
        }

        private void Circle_Radius_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double radius = double.Parse(CircleRadius.Text);
                _shapeManager.SetCircleRadius(radius);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void Shape_SetPosition()
        {
            if (_mouseDownOnCanvas || _shapeManager.ModifyingShape)
            {
                return;
            }

            try
            {
                double x = double.Parse(XPosition.Text);
                double y = double.Parse(YPosition.Text);
                _shapeManager.SetPosition(x, y);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void Line_SetParameters()
        {
            try
            {
                double x1 = double.Parse(LineX1.Text);
                double y1 = double.Parse(LineY1.Text);
                double x2 = double.Parse(LineX2.Text);
                double y2 = double.Parse(LineY2.Text);
                _shapeManager.SetLineParameters(x1, y1, x2, y2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void Rectangle_SetWidthAndHeight()
        {
            try
            {
                double width = double.Parse(RectangleWidth.Text);
                double height = double.Parse(RectangleHeight.Text);
                _shapeManager.SetWidthAndHeight(width, height);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void UpdateShapeSpecialInformation(Shape shape)
        {
            switch (shape)
            {
                case Line line:
                    LineX1.Text = Math.Round(line.X1, 2).ToString();
                    LineY1.Text = Math.Round(line.Y1, 2).ToString();
                    LineX2.Text = Math.Round(line.X2, 2).ToString();
                    LineY2.Text = Math.Round(line.Y2, 2).ToString();
                    break;
                case Rectangle rectangle:
                    RectangleWidth.Text = Math.Round(rectangle.Width, 2).ToString();
                    RectangleHeight.Text = Math.Round(rectangle.Height, 2).ToString();
                    break;
                case Ellipse circle:
                    CircleRadius.Text = Math.Round(circle.Width / 2, 2).ToString();
                    break;
            }
        }

        #endregion
        #endregion

        #region FileManagement 

        private async void OpenImgButton_OnClick(object sender, RoutedEventArgs e)
        {
            await _imageManager.Open();
        }

        private async void SaveImgButton_OnClick(object sender, RoutedEventArgs e)
        {
            await _imageManager.Save();
        }

        #endregion
    }
}
