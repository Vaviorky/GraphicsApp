using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.Practices.Prism.Mvvm;

namespace GraphicsProject.Classes.ColorPicker
{
    public partial class ColorPickerViewModel : BindableBase
    {
        private static readonly double PickerWidth = 250d;
        private static readonly double PickerHeight = 250d;
        private static readonly ColorConverter Converter = new ColorConverter();

        public ColorPickerViewModel()
        {
            color = "#FFFF0000";
            pickPointX = 150d;
            pickPointY = 0d;
            colorSpectrumPoint = 0d;
            UpdateColor(Colors.Red);
            UpdatePickPoint();
        }

        private void UpdateColor(Color color, bool updateCmyk = true)
        {
            this.color = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            red = color.R;
            green = color.G;
            blue = color.B;

            if (updateCmyk)
            {
                black = Math.Min(1 - red / 256f, Math.Min(1 - green / 256f, 1 - blue / 256f));
                cyan = (1 - red / 256f - black) / (1 - black);
                magenta = (1 - green / 256f - black) / (1 - black);
                yellow = (1 - blue / 256f - black) / (1 - black);
            }

            var hsv = ToHsv(color);
            var h = FromHsv(hsv[0], 1f, 1f);
            hueColor = string.Format("#FF{0:X2}{1:X2}{2:X2}", h.R, h.G, h.B);

            OnPropertyChanged("Color");

            OnPropertyChanged("Red");
            OnPropertyChanged("Green");
            OnPropertyChanged("Blue");

            OnPropertyChanged("Cyan");
            OnPropertyChanged("Magenta");
            OnPropertyChanged("Yellow");
            OnPropertyChanged("Black");

            OnPropertyChanged("HueColor");
        }

        private void UpdatePickPoint()
        {
            var hsv = ToHsv((Color)Converter.Convert(color, typeof(Color), null, null));
            pickPointX = PickerWidth * hsv[1];
            pickPointY = PickerHeight * (1 - hsv[2]);
            colorSpectrumPoint = PickerHeight * hsv[0] / 360f;

            OnPropertyChanged("PickPointX");
            OnPropertyChanged("PickPointY");
            OnPropertyChanged("ColorSpectrumPoint");
        }

        private static float[] ToHsv(Color color)
        {
            var rgb = new[]
            {
                color.R / 255f, color.G / 255f, color.B / 255f
            };

            float max = rgb.Max();
            float min = rgb.Min();

            float h, s, v;
            if (max == min)
            {
                h = 0f;
            }
            else if (max == rgb[0])
            {
                h = (60f * (rgb[1] - rgb[2]) / (max - min) + 360f) % 360f;
            }
            else if (max == rgb[1])
            {
                h = 60f * (rgb[2] - rgb[0]) / (max - min) + 120f;
            }
            else
            {
                h = 60f * (rgb[0] - rgb[1]) / (max - min) + 240f;
            }

            if (max == 0d)
            {
                s = 0f;
            }
            else
            {
                s = (max - min) / max;
            }
            v = max;

            return new[] { h, s, v };
        }

        private static Color FromHsv(float hue, float saturation, float brightness)
        {
            if (saturation == 0)
            {
                var c = (byte)Math.Round(brightness * 255f, MidpointRounding.AwayFromZero);
                return ColorHelper.FromArgb(0xff, c, c, c);
            }

            var hi = ((int)(hue / 60f)) % 6;
            var f = hue / 60f - (int)(hue / 60d);
            var p = brightness * (1 - saturation);
            var q = brightness * (1 - f * saturation);
            var t = brightness * (1 - (1 - f) * saturation);

            float r, g, b;
            switch (hi)
            {
                case 0:
                    r = brightness;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = brightness;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = brightness;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = brightness;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = brightness;
                    break;

                case 5:
                    r = brightness;
                    g = p;
                    b = q;
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return ColorHelper.FromArgb(
                0xff,
                (byte)Math.Round(r * 255d),
                (byte)Math.Round(g * 255d),
                (byte)Math.Round(b * 255d));
        }

        partial void OnColorChanged()
        {
            UpdateColor((Color)Converter.Convert(color, typeof(Color), null, null));
            UpdatePickPoint();
        }

        partial void OnRedChanged()
        {
            var updated = (Color)Converter.Convert(color, typeof(Color), null, null);
            updated.R = (byte)Math.Max(0, red);
            updated.R = Math.Min((byte)0xff, updated.R);
            UpdateColor(updated);
            UpdatePickPoint();
        }

        partial void OnGreenChanged()
        {
            var updated = (Color)Converter.Convert(color, typeof(Color), null, null);
            updated.G = (byte)Math.Max(0, green);
            updated.G = Math.Min((byte)0xff, updated.G);
            UpdateColor(updated);
            UpdatePickPoint();
        }

        partial void OnBlueChanged()
        {
            var updated = (Color)Converter.Convert(color, typeof(Color), null, null);
            updated.B = (byte)Math.Max(0, blue);
            updated.B = Math.Min((byte)0xff, updated.B);
            UpdateColor(updated);
            UpdatePickPoint();
        }

        partial void OnCyanChanged()
        {
            UpdateCmyk();
        }

        partial void OnMagentaChanged()
        {
            UpdateCmyk();
        }

        partial void OnYellowChanged()
        {
            UpdateCmyk();
        }

        partial void OnBlackChanged()
        {
            UpdateCmyk();
        }

        private void UpdateCmyk()
        {
            var tempRed = 1 - Math.Min(1, cyan * (1 - black) + black) * 256;
            var tempGreen = 1 - Math.Min(1, magenta * (1 - black) + black) * 256;
            var tempBlue = 1 - Math.Min(1, yellow * (1 - black) + black) * 256;
            var updated = new Color
            {
                R = (byte)tempRed,
                G = (byte)tempGreen,
                B = (byte)tempBlue
            };
            UpdateColor(updated, false);
            UpdatePickPoint();
        }

        partial void OnColorSpectrumPointChanged()
        {
            var old = (Color)Converter.Convert(color, typeof(Color), null, null);
            var hsv = ToHsv(old);
            hsv[0] = (float)(colorSpectrumPoint * 360f / PickerHeight);
            var updated = FromHsv(hsv[0], hsv[1], hsv[2]);
            updated.A = old.A;
            UpdateColor(updated);

            var h = FromHsv(hsv[0], 1f, 1f);
            HueColor = string.Format("#FF{0:X2}{1:X2}{2:X2}", h.R, h.G, h.B);
        }

        public void OnPickPointChanged()
        {
            var old = (Color)Converter.Convert(hueColor, typeof(Color), null, null);
            var hsv = ToHsv(old);
            var updated = FromHsv(hsv[0], (float)(pickPointX / PickerWidth),
                1f - (float)(pickPointY / PickerHeight));
            updated.A = old.A;
            UpdateColor(updated);
        }
    }
}