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
        private Histogram histogram = new Histogram();
        private Brush brush;

        public HistogramWindow(Brush brush)
        {
            this.brush = brush;
            this.InitializeComponent();
            
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
