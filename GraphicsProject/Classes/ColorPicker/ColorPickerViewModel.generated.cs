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

        private int cyan;

        partial void OnCyanChanging(ref int value);

        partial void OnCyanChanged();

        public int Cyan
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
                    this.SetProperty<int>(ref this.cyan, value);
                    this.OnCyanChanged();
                }
            }
        }

        #endregion

        #region Magenta

        private int magenta;

        partial void OnMagentaChanging(ref int value);

        partial void OnMagentaChanged();

        public int Magenta
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
                    this.SetProperty<int>(ref this.magenta, value);
                    this.OnMagentaChanged();
                }
            }
        }

        #endregion

        #region Yellow

        private int yellow;

        partial void OnYellowChanging(ref int value);

        partial void OnYellowChanged();

        public int Yellow
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
                    this.SetProperty<int>(ref this.yellow, value);
                    this.OnYellowChanged();
                }
            }
        }

        #endregion

        #region Black

        private int black;

        partial void OnBlackChanging(ref int value);

        partial void OnBlackChanged();

        public int Black
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
                    this.SetProperty<int>(ref this.black, value);
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

        #region RedStartColor

        private string redStartColor;

        partial void OnRedStartColorChanging(ref string value);

        partial void OnRedStartColorChanged();

        public string RedStartColor
        {
            get
            {
                return this.redStartColor;
            }

            set
            {
                if (this.redStartColor != value)
                {
                    this.OnRedStartColorChanging(ref value);
                    this.SetProperty<string>(ref this.redStartColor, value);
                    this.OnRedStartColorChanged();
                }
            }
        }
        #endregion

        #region RedEndColor

        private string redEndColor;

        partial void OnRedEndColorChanging(ref string value);

        partial void OnRedEndColorChanged();

        public string RedEndColor
        {
            get
            {
                return this.redEndColor;
            }

            set
            {
                if (this.redEndColor != value)
                {
                    this.OnRedEndColorChanging(ref value);
                    this.SetProperty<string>(ref this.redEndColor, value);
                    this.OnRedEndColorChanged();
                }
            }
        }
        #endregion

        #region GreenStartColor

        private string greenStartColor;

        partial void OnGreenStartColorChanging(ref string value);

        partial void OnGreenStartColorChanged();

        public string GreenStartColor
        {
            get
            {
                return this.greenStartColor;
            }

            set
            {
                if (this.greenStartColor != value)
                {
                    this.OnGreenStartColorChanging(ref value);
                    this.SetProperty<string>(ref this.greenStartColor, value);
                    this.OnGreenStartColorChanged();
                }
            }
        }
        #endregion 

        #region GreenEndColor

        private string greenEndColor;

        partial void OnGreenEndColorChanging(ref string value);

        partial void OnGreenEndColorChanged();

        public string GreenEndColor
        {
            get
            {
                return this.greenEndColor;
            }

            set
            {
                if (this.greenEndColor != value)
                {
                    this.OnGreenEndColorChanging(ref value);
                    this.SetProperty<string>(ref this.greenEndColor, value);
                    this.OnGreenEndColorChanged();
                }
            }
        }
        #endregion

        #region BlueStartColor

        private string blueStartColor;

        partial void OnBlueStartColorChanging(ref string value);

        partial void OnBlueStartColorChanged();

        public string BlueStartColor
        {
            get
            {
                return this.blueStartColor;
            }

            set
            {
                if (this.blueStartColor != value)
                {
                    this.OnBlueStartColorChanging(ref value);
                    this.SetProperty<string>(ref this.blueStartColor, value);
                    this.OnBlueStartColorChanged();
                }
            }
        }
        #endregion

        #region BlueEndColor

        private string blueEndColor;

        partial void OnBlueEndColorChanging(ref string value);

        partial void OnBlueEndColorChanged();

        public string BlueEndColor
        {
            get
            {
                return this.blueEndColor;
            }

            set
            {
                if (this.blueEndColor != value)
                {
                    this.OnBlueEndColorChanging(ref value);
                    this.SetProperty<string>(ref this.blueEndColor, value);
                    this.OnBlueEndColorChanged();
                }
            }
        }
        #endregion

        #region AlphaStartColor

        private string alphaStartColor;

        partial void OnAlphaStartColorChanging(ref string value);

        partial void OnAlphaStartColorChanged();

        public string AlphaStartColor
        {
            get
            {
                return this.alphaStartColor;
            }

            set
            {
                if (this.alphaStartColor != value)
                {
                    this.OnAlphaStartColorChanging(ref value);
                    this.SetProperty<string>(ref this.alphaStartColor, value);
                    this.OnAlphaStartColorChanged();
                }
            }
        }
        #endregion

        #region AlphaEndColor

        private string alphaEndColor;

        partial void OnAlphaEndColorChanging(ref string value);

        partial void OnAlphaEndColorChanged();

        public string AlphaEndColor
        {
            get
            {
                return this.alphaEndColor;
            }

            set
            {
                if (this.alphaEndColor != value)
                {
                    this.OnAlphaEndColorChanging(ref value);
                    this.SetProperty<string>(ref this.alphaEndColor, value);
                    this.OnAlphaEndColorChanged();
                }
            }
        }
        #endregion 
    }
}
