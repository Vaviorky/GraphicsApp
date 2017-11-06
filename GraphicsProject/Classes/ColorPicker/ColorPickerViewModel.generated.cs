using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsProject.Classes.ColorPicker
{
    public partial class ColorPickerViewModel : BindableBase
    {
        #region Color

        private string color;

        partial void OnColorChanging(ref string value);

        partial void OnColorChanged();

        public string Color
        {
            get
            {
                return this.color;
            }

            set
            {
                if (this.color != value)
                {
                    this.OnColorChanging(ref value);
                    this.SetProperty<string>(ref this.color, value);
                    this.OnColorChanged();
                }
            }
        }
        #endregion

        #region Red
        private int red;

        partial void OnRedChanging(ref int value);

        partial void OnRedChanged();

        public int Red
        {
            get
            {
                return this.red;
            }

            set
            {
                if (this.red != value)
                {
                    this.OnRedChanging(ref value);
                    this.SetProperty<int>(ref this.red, value);
                    this.OnRedChanged();
                }
            }
        }
        #endregion

        #region Green
        private int green;

        partial void OnGreenChanging(ref int value);

        partial void OnGreenChanged();

        public int Green
        {
            get
            {
                return this.green;
            }

            set
            {
                if (this.green != value)
                {
                    this.OnGreenChanging(ref value);
                    this.SetProperty<int>(ref this.green, value);
                    this.OnGreenChanged();
                }
            }
        }
        #endregion

        #region Blue
        private int blue;

        partial void OnBlueChanging(ref int value);

        partial void OnBlueChanged();

        public int Blue
        {
            get
            {
                return this.blue;
            }

            set
            {
                if (this.blue != value)
                {
                    this.OnBlueChanging(ref value);
                    this.SetProperty<int>(ref this.blue, value);
                    this.OnBlueChanged();
                }
            }
        }
        #endregion

        #region Cyan

        private float cyan;

        partial void OnCyanChanging(ref float value);

        partial void OnCyanChanged();

        public float Cyan
        {
            get
            {
                return this.cyan;
            }

            set
            {
                if (this.cyan != value)
                {
                    this.OnCyanChanging(ref value);
                    this.SetProperty<float>(ref this.cyan, value);
                    this.OnCyanChanged();
                }
            }
        }

        #endregion

        #region Magenta

        private float magenta;

        partial void OnMagentaChanging(ref float value);

        partial void OnMagentaChanged();

        public float Magenta
        {
            get
            {
                return this.magenta;
            }

            set
            {
                if (this.magenta != value)
                {
                    this.OnMagentaChanging(ref value);
                    this.SetProperty<float>(ref this.magenta, value);
                    this.OnMagentaChanged();
                }
            }
        }

        #endregion

        #region Yellow

        private float yellow;

        partial void OnYellowChanging(ref float value);

        partial void OnYellowChanged();

        public float Yellow
        {
            get
            {
                return this.yellow;
            }

            set
            {
                if (this.yellow != value)
                {
                    this.OnYellowChanging(ref value);
                    this.SetProperty<float>(ref this.yellow, value);
                    this.OnYellowChanged();
                }
            }
        }

        #endregion

        #region Black

        private float black;

        partial void OnBlackChanging(ref float value);

        partial void OnBlackChanged();

        public float Black
        {
            get
            {
                return this.black;
            }

            set
            {
                if (this.black != value)
                {
                    this.OnBlackChanging(ref value);
                    this.SetProperty<float>(ref this.black, value);
                    this.OnBlackChanged();
                }
            }
        }

        #endregion

        #region PickPointX

        private double pickPointX;

        partial void OnPickPointXChanging(ref double value);

        partial void OnPickPointXChanged();

        public double PickPointX
        {
            get
            {
                return this.pickPointX;
            }

            set
            {
                if (this.pickPointX != value)
                {
                    this.OnPickPointXChanging(ref value);
                    this.SetProperty<double>(ref this.pickPointX, value);
                    this.OnPickPointXChanged();
                }
            }
        }
        #endregion

        #region PickPointY

        private double pickPointY;

        partial void OnPickPointYChanging(ref double value);

        partial void OnPickPointYChanged();

        public double PickPointY
        {
            get
            {
                return this.pickPointY;
            }

            set
            {
                if (this.pickPointY != value)
                {
                    this.OnPickPointYChanging(ref value);
                    this.SetProperty<double>(ref this.pickPointY, value);
                    this.OnPickPointYChanged();
                }
            }
        }
        #endregion

        #region HueColor

        private string hueColor;

        partial void OnHueColorChanging(ref string value);

        partial void OnHueColorChanged();

        public string HueColor
        {
            get
            {
                return this.hueColor;
            }

            set
            {
                if (this.hueColor != value)
                {
                    this.OnHueColorChanging(ref value);
                    this.SetProperty<string>(ref this.hueColor, value);
                    this.OnHueColorChanged();
                }
            }
        }
        #endregion

        #region ColorSpectrumPoint

        private double colorSpectrumPoint;

        partial void OnColorSpectrumPointChanging(ref double value);

        partial void OnColorSpectrumPointChanged();

        public double ColorSpectrumPoint
        {
            get
            {
                return this.colorSpectrumPoint;
            }

            set
            {
                if (this.colorSpectrumPoint != value)
                {
                    this.OnColorSpectrumPointChanging(ref value);
                    this.SetProperty<double>(ref this.colorSpectrumPoint, value);
                    this.OnColorSpectrumPointChanged();
                }
            }
        }
        #endregion
    }
}
