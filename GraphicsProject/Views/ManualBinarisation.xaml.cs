using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using GraphicsProject.Classes.Binarisation;

namespace GraphicsProject.Views
{
    public sealed partial class ManualBinarisation : ContentDialog
    {
        public int Threshold { get; private set; }
        private readonly ImageBrush _origin;

        public ManualBinarisation(ImageBrush image)
        {
            _origin = image;
            InitializeComponent();
            UpdatePreviewImage(125);
        }

        private async void UpdatePreviewImage(int threshold)
        {
            PreviewImage.Background = await BinarisationHelper.ManualBinarisationPreview(_origin, threshold);
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var newValue = int.Parse(ThresholdTextBox.Text);
                if (newValue < 0 || newValue > 255) return;

                ThresholdSlider.Value = newValue;
                Threshold = newValue;
                UpdatePreviewImage(newValue);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("[TextBox_OnTextChanged] " + exception.Message);
            }
        }

        private void Slider_OnValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ThresholdTextBox.Text = ((int)e.NewValue).ToString();
        }
    }
}
