using GraphicsProject.Classes.Histograms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GraphicsProject.Views
{
    public sealed partial class HistogramWindow : ContentDialog
    {
        public PointCollection HistogramRed => HistogramHelper.ConvertToPointCollection(Histogram.HistogramR);
        public PointCollection HistogramGreen => HistogramHelper.ConvertToPointCollection(Histogram.HistogramG);
        public PointCollection HistogramBlue => HistogramHelper.ConvertToPointCollection(Histogram.HistogramB);
        public PointCollection HistogramAv => HistogramHelper.ConvertToPointCollection(Histogram.HistogramAv);

        public ImageBrush Image { get; }

        public Histogram Histogram { get; }

        public HistogramWindow(ImageBrush image)
        {
            Image = image;
            this.InitializeComponent();
            Histogram = new Histogram(Image);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
