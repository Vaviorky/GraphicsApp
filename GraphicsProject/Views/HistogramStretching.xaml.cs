using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using GraphicsProject.Classes.Histograms;
using GraphicsProject.Enums;

namespace GraphicsProject.Views
{
    public sealed partial class HistogramStretching : ContentDialog
    {
        public ImageBrush Image { get; private set; }

        private readonly Histogram _histogram;
        private WriteableBitmap _bitmap;
        public HistogramType HistogramType = HistogramType.Red;
        public int[] Lut { get; private set; }

        private readonly ImageBrush _origin;

        public HistogramStretching(ImageBrush image)
        {
            _origin = image;
            this.InitializeComponent();
            InitializePreview(image);
            _histogram = new Histogram(Image);
            InitializeHistogram();
        }

        private async void InitializePreview(ImageBrush image)
        {
            Image = new ImageBrush();

            var wb = (WriteableBitmap)image.ImageSource;
            _bitmap = new WriteableBitmap(wb.PixelWidth, wb.PixelHeight);
            var data = wb.PixelBuffer.ToArray();
            using (Stream stream = _bitmap.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            Image.ImageSource = _bitmap;
        }

        private void InitializeHistogram()
        {
            HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(_histogram.HistogramR);
            HistogramPolygon.Fill = new SolidColorBrush(Colors.Red);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = (ComboBox)sender;
            var selectedItem = (ComboBoxItem)combobox.SelectedItem;

            if (selectedItem == null || HistogramPolygon == null) return;

            switch (selectedItem.Content.ToString())
            {
                default:
                case "Czerwony":
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(_histogram.HistogramR);
                    HistogramPolygon.Fill = new SolidColorBrush(Colors.Red);
                    HistogramType = HistogramType.Red;
                    break;
                case "Zielony":
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(_histogram.HistogramG);
                    HistogramPolygon.Fill = new SolidColorBrush(Colors.Green);
                    HistogramType = HistogramType.Green;
                    break;
                case "Niebieski":
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(_histogram.HistogramB);
                    HistogramPolygon.Fill = new SolidColorBrush(Colors.Blue);
                    HistogramType = HistogramType.Blue;
                    break;
                case "Uśredniony":
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(_histogram.HistogramAv);
                    HistogramPolygon.Fill = new SolidColorBrush(Colors.Gray);
                    HistogramType = HistogramType.Average;
                    break;
            }
        }

        private async void HistogramValue_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(HistogramMin.Text) || string.IsNullOrEmpty(HistogramMax.Text)) return;

            try
            {
                int min = int.Parse(HistogramMin.Text);
                int max = int.Parse(HistogramMax.Text);

                if (min < 0 || min > 255 || max < 0 || max > 255 || max <= min) return;

                await UpdatePreviewImage(min, max);
                UpdateHistogram();
            }
            catch (Exception exception)
            {
                Debug.WriteLine("[HistogramValue_OnTextChanged] " + exception.Message);
            }
        }

        private async Task UpdatePreviewImage(int min, int max)
        {
            Lut = HistogramHelper.GetLutStretching(min, max);
            PreviewImage.Background = await HistogramHelper.StretchHistogram(_origin, Lut, HistogramType);
        }

        private void UpdateHistogram()
        {
            var tempHistogram = new Histogram((ImageBrush) PreviewImage.Background);

            switch (HistogramType)
            {
                default:
                case HistogramType.Red:
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(tempHistogram.HistogramR);
                    break;
                case HistogramType.Green:
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(tempHistogram.HistogramG);
                    break;
                case HistogramType.Blue:
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(tempHistogram.HistogramB);
                    break;
                case HistogramType.Average:
                    HistogramPolygon.Points = HistogramHelper.ConvertToPointCollection(tempHistogram.HistogramAv);
                    break;
            }
        }
    }
}
