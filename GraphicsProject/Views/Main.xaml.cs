using System;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using GraphicsProject.Classes.ColorPicker;
using GraphicsProject.Classes.ImageManagement;
using GraphicsProject.Classes.Shapes;
using GraphicsProject.Classes;
using GraphicsProject.Classes.Binarisation;
using GraphicsProject.Classes.Histograms;
using GraphicsProject.Classes.ImageProcessing;
using GraphicsProject.Enums;

namespace GraphicsProject.Views
{
    public sealed partial class Main : Page
    {
        public ColorPickerViewModel ViewModel { get; } = new ColorPickerViewModel();

        private readonly ImageManager _imageManager;
        private ShapeManager _shapeManager;

        private ShapeType _shapeType;

        private Point _startingPoint;
        private Point _endingPoint;
        private bool _canStartCreatingShape = true;
        private bool _hasShapeBeenSelected;
        private bool _mouseDownOnCanvas;

        public Main()
        {
            InitializeComponent();
            _imageManager = new ImageManager(DrawingCanvas);
            InitializeShapeManager();
            DataContext = ViewModel;
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
            _shapeManager = new ShapeManager(DrawingCanvas);
            _shapeManager.OnShapeSelection += OnShapeSelection;
            _shapeManager.OnShapeParameterChange += UpdateTextFields;
            _shapeManager.OnShapeModificationComplete += ResetDrawing;
            _shapeType = ShapeType.Line;
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
            JPEGCompression compression = new JPEGCompression();

            var result = await compression.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await _imageManager.Save(compression.CompressionValue);
            }
        }

        private void ExitAppButton_OnClick(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        #endregion

        #region ColorPicker

        private void OnPickerPressed(object sender, PointerRoutedEventArgs e)
        {
            PickColor(e.GetCurrentPoint(PickerCanvas).Position);
            PickerCanvas.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                PickColor(args.GetCurrentPoint(PickerCanvas).Position);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                PickerCanvas.ReleasePointerCapture(args.Pointer);
                PickColor(args.GetCurrentPoint(PickerCanvas).Position);
                PickerCanvas.PointerMoved -= Moved;
                PickerCanvas.PointerReleased -= Released;
            }

            PickerCanvas.PointerMoved += Moved;
            PickerCanvas.PointerReleased += Released;
        }

        private void OnHuePressed(object sender, PointerRoutedEventArgs e)
        {
            ChangeHue(e.GetCurrentPoint(ColorSpectrum).Position.Y);
            ColorSpectrum.CapturePointer(e.Pointer);

            void Moved(object s, PointerRoutedEventArgs args)
            {
                ChangeHue(args.GetCurrentPoint(ColorSpectrum).Position.Y);
            }

            void Released(object s, PointerRoutedEventArgs args)
            {
                ColorSpectrum.ReleasePointerCapture(args.Pointer);
                ChangeHue(args.GetCurrentPoint(ColorSpectrum).Position.Y);
                ColorSpectrum.PointerMoved -= Moved;
                ColorSpectrum.PointerReleased -= Released;
            }

            ColorSpectrum.PointerMoved += Moved;
            ColorSpectrum.PointerReleased += Released;
        }

        private void ChangeHue(double y)
        {
            var py = Math.Max(0d, y);
            py = Math.Min(ColorSpectrum.ActualHeight, py);

            ViewModel.ColorSpectrumPoint = Math.Round(py, MidpointRounding.AwayFromZero);
        }

        private void PickColor(Point point)
        {
            var px = Math.Max(0d, point.X);
            px = Math.Min(PickerCanvas.ActualWidth, px);
            var py = Math.Max(0d, point.Y);
            py = Math.Min(PickerCanvas.ActualHeight, py);

            ViewModel.PickPointX = Math.Round(px, MidpointRounding.AwayFromZero);
            ViewModel.PickPointY = Math.Round(py, MidpointRounding.AwayFromZero);
            ViewModel.OnPickPointChanged();
        }

        #endregion

        #region ImageTransformation

        private void OriginalPicture_OnClick(object sender, RoutedEventArgs e)
        {
            _imageManager.RevertToOriginalImage();
        }

        private async void AddValueItem_OnClick(object sender, RoutedEventArgs e)
        {
            PixelManipulation manipulation = new PixelManipulation("Podaj wartość do dodania:");

            var result = await manipulation.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DrawingCanvas.Background.AddPixelByValue(manipulation.PixelManipulationValue);
            }
        }

        private async void SubstractValueItem_OnClick(object sender, RoutedEventArgs e)
        {
            PixelManipulation manipulation = new PixelManipulation("Podaj wartość do odjęcia:");

            var result = await manipulation.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DrawingCanvas.Background.AddPixelByValue(-manipulation.PixelManipulationValue);
            }
        }

        private async void MultiplyValueItem_OnClick(object sender, RoutedEventArgs e)
        {
            PixelManipulation manipulation = new PixelManipulation("Podaj wartość do pomnożenia:");

            var result = await manipulation.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DrawingCanvas.Background.MultiplyPixelByValue(manipulation.PixelManipulationValue);
            }
        }

        private async void DivideValueItem_OnClick(object sender, RoutedEventArgs e)
        {
            PixelManipulation manipulation = new PixelManipulation("Podaj wartość do podzielenia:");

            var result = await manipulation.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (manipulation.PixelManipulationValue == 0)
                {
                    return;
                }

                DrawingCanvas.Background.MultiplyPixelByValue(1 / manipulation.PixelManipulationValue);
            }
        }

        private void GrayscaleOneItem_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.MakeGrayscale_FirstWay();
        }

        private void GrayscaleTwoItem_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.MakeGrayscale_SecondWay();
        }

        private async void BrightnessItem_OnClick(object sender, RoutedEventArgs e)
        {
            PixelManipulation manipulation = new PixelManipulation("Podaj wartość do jasności:");

            var result = await manipulation.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DrawingCanvas.Background.AdjustBrightness((float)manipulation.PixelManipulationValue);
            }
        }

        private void Mean_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.MeanFilter();
        }

        private void Median_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.MedianFilter();
        }

        private void Sharp_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.SharpFilter();
        }

        private void Sobel_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.SobelFilter();
        }

        private void Gauss_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.GaussFilter();
        }
        
        #endregion

        #region Histograms

        private async void ShowHistogram_OnClick(object sender, RoutedEventArgs e)
        {
            var img = (ImageBrush) DrawingCanvas.Background;
            img.Stretch = Stretch.None;

            var histogramWindow = new HistogramWindow(img)
            {
                MinWidth = this.ActualWidth,
                MaxWidth = this.ActualWidth,
                MinHeight = this.ActualHeight,
                MaxHeight = this.ActualHeight
            };
            await histogramWindow.ShowAsync();
        }

        private void HistogramEqualisation_OnClick(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Background.EqualizeHistogram();
        }

        private async void HistogramStretching_OnClick(object sender, RoutedEventArgs e)
        {
            var hs = new HistogramStretching((ImageBrush) DrawingCanvas.Background);
            var result = await hs.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DrawingCanvas.Background.StretchHistogram(hs.Lut, hs.HistogramType);
            }
        }

        #endregion

        #region Binarisation

        private async void ManualBinarisation_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DrawingCanvas.Background.IsGrayscaled())
            {
                var dialog = new MessageDialog("Obraz musi być czarno-biały!", "Błąd");
                await dialog.ShowAsync();
                return;
            }

            var mb = new ManualBinarisation((ImageBrush) DrawingCanvas.Background);
            var result = await mb.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                DrawingCanvas.Background.MakeBinarisation(mb.Threshold);
            }
        }

        #endregion


    }
}
