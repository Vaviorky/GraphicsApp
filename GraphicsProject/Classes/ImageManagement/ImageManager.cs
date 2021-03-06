﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GraphicsProject.Enums;

namespace GraphicsProject.Classes.ImageManagement
{
    internal class ImageManager
    {
        private ImageBrush _actualImage;
        private readonly Canvas _canvas;
        private string _p3Line = "";
        private WriteableBitmap _bitmap;

        private IStorageFile _file;

        public ImageManager(Canvas canvas)
        {
            _canvas = canvas;
        }

        #region Open/Save

        public async Task Open()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".ppm");

            _file = await picker.PickSingleFileAsync();

            if (_file != null)
            {
                CheckForImageToLoad(_file);
            }
        }

        private async void CheckForImageToLoad(IStorageFile file)
        {
            var fileType = file.FileType;
            if (fileType.Equals(".jpg") || fileType.Equals(".jpeg"))
            {
                await LoadJpg(file);
            }
            else if (fileType.Equals(".ppm"))
            {
                await LoadPpm(file);
            }
        }

        public async Task Save(double compression)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Obraz JPEG", new[] { ".jpg" });
            var file = await picker.PickSaveFileAsync();

            if (file == null || _bitmap == null) return;

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var propertySet = new BitmapPropertySet();
                var qualityValue = new BitmapTypedValue(compression / 10, PropertyType.Single);

                propertySet.Add("ImageQuality", qualityValue);

                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream, propertySet);
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.NearestNeighbor;
                Stream pixelStream = _bitmap.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                    (uint)_bitmap.PixelWidth, (uint)_bitmap.PixelHeight, 96, 96, pixels);

                await encoder.FlushAsync();
            }
        }

        #endregion

        #region LoadJpg

        private async Task LoadJpg(IStorageFile file)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                _bitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                await _bitmap.SetSourceAsync(stream);

                _actualImage = new ImageBrush { ImageSource = _bitmap };
                _canvas.Background = _actualImage;
                _canvas.Width = decoder.PixelWidth;
                _canvas.Height = decoder.PixelHeight;
            }
        }

        #endregion

        #region LoadPPM

        private async Task LoadPpm(IStorageFile file)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            using (var binaryReader = new BinaryReader(stream.AsStream()))
            {
                var ppmType = binaryReader.ReadBytes(2);

                if (ppmType[0] == 'P' && ppmType[1] == '3')
                {
                    await LoadP3(new StreamReader(stream.AsStream()));
                }
                else if (ppmType[0] == 'P' && ppmType[1] == '6')
                {
                    await LoadP6(binaryReader);
                }
            }
        }

        #endregion

        #region LoadP3

        private async Task LoadP3(StreamReader reader)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            reader.ReadLine();

            var parameters = LoadParameters(reader);

            if (parameters.Length == 0) return;

            int width = parameters[0];
            int height = parameters[1];
            int max = parameters[2];

            int[] rawData = ReadP3ImgData(reader, parameters[0], parameters[1]);
            byte[] preparedData = LoadP3Img(width, height, max, rawData);

            await UpdateCanvas(preparedData, width, height);
            stopwatch.Stop();

            await ShowElapsedTime(stopwatch.ElapsedMilliseconds);
        }

        private int[] LoadParameters(StreamReader reader)
        {
            int width = -1;
            int height = -1;
            int max = -1;

            do
            {
                var line = reader.ReadLine().Replace('\t', ' ');
                var words = line.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    var word = words[i];
                    if (word.IndexOf('#') != -1)
                    {
                        break;
                    }

                    if (IsCharacterInString(word))
                    {
                        continue;
                    }

                    try
                    {
                        var number = ParseInt(word);
                        if (width == -1)
                        {
                            width = number;
                        }
                        else if (height == -1)
                        {
                            height = number;
                        }
                        else if (max == -1)
                        {
                            max = number;
                            for (int j = i + word.Length; j < words.Length; j++)
                            {
                                var newWord = words[j];

                                if (newWord.IndexOf('#') != -1)
                                {
                                    break;
                                }

                                if (IsCharacterInString(newWord))
                                {
                                    continue;
                                }
                                _p3Line += words[j];
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        ShowErrorMessage(ImageLoadError.Exception, e);
                        return new[] { 0, 0, 1 };
                    }
                }
            } while (max == -1);

            return new[] { width, height, max };
        }

        private int[] ReadP3ImgData(StreamReader reader, int width, int height)
        {
            int[] data = new int[width * height * 3];
            int counter = 0;

            var splitter = new FastSplit(1);

            if (_p3Line.Length != 0)
            {
                data[counter++] = ParseInt(_p3Line);
            }

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (line.IndexOf('\t') != -1)
                {
                    line = line.Replace('\t', ' ');
                }

                var length = splitter.SafeSplit(line, ' ');
                var words = splitter.Results;

                for (var i = 0; i < length; i++)
                {
                    var word = words[i];
                    if (word.IndexOf('#') != -1) break;
                    if (IsCharacterInString(word)) continue;

                    data[counter++] = ParseInt(word);
                }
            }

            return data;
        }

        private byte[] LoadP3Img(int width, int height, int max, int[] numbers)
        {
            byte[] array = new byte[height * width * 4];
            try
            {
                var length = array.Length;
                for (int i = 0, j = 0; i < length; i += 4, j += 3)
                {
                    array[i] = (byte)(numbers[j + 2] * 255 / max);
                    array[i + 1] = (byte)(numbers[j + 1] * 255 / max);
                    array[i + 2] = (byte)(numbers[j] * 255 / max);
                    array[i + 3] = 255;
                }
            }
            catch (Exception e)
            {
                ShowErrorMessage(ImageLoadError.Exception, e);
                return null;
            }
            return array;
        }

        private static int ParseInt(string str)
        {
            int temp = 0;
            var length = str.Length;
            for (int i = 0; i < length; i++)
                temp = temp * 10 + (str[i] - '0');

            return temp;
        }

        private bool IsCharacterInString(string str)
        {
            if (str.Length == 0) return true;

            for (int j = 0; j < str.Length; j++)
            {
                switch (str[j])
                {
                    case '\n':
                        return true;
                    case '\0':
                        return true;
                    case '\t':
                        return true;
                    case ' ':
                        return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        #endregion

        #region LoadP6

        private async Task LoadP6(BinaryReader reader)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ReadWhitespace(reader);
            int width = ReadValue(reader);
            ReadWhitespace(reader);
            int height = ReadValue(reader);
            ReadWhitespace(reader);
            int maximumColorValue = ReadValue(reader);

            byte[] array = ReadP6Array(reader, width, height, maximumColorValue);

            await UpdateCanvas(array, width, height);

            stopwatch.Stop();

            await ShowElapsedTime(stopwatch.ElapsedMilliseconds);
        }

        private byte[] ReadP6Array(BinaryReader reader, int width, int height, int max)
        {
            byte[] array = new byte[height * width * 4];
            try
            {
                byte[] rgb;
                short rInt16, gInt16, bInt16;
                for (int i = 0; i < array.Length; i += 4)
                {
                    if (max > 255)
                    {
                        rgb = reader.ReadBytes(6);
                        rInt16 = BitConverter.ToInt16(rgb, 0);
                        gInt16 = BitConverter.ToInt16(rgb, 2);
                        bInt16 = BitConverter.ToInt16(rgb, 4);
                    }
                    else
                    {
                        rgb = reader.ReadBytes(3);
                        rInt16 = rgb[0];
                        gInt16 = rgb[1];
                        bInt16 = rgb[2];
                    }
                    array[i] = (byte)((bInt16 * 255) / max);
                    array[i + 1] = (byte)((gInt16 * 255) / max);
                    array[i + 2] = (byte)((rInt16 * 255) / max);
                    array[i + 3] = 255;
                }
            }
            catch (Exception e)
            {
                ShowErrorMessage(ImageLoadError.Exception, e);
            }

            return array;
        }

        private static void ReadWhitespace(BinaryReader reader)
        {
            char c = reader.ReadChar();

            while (c == ' ' || c == '\t' || c == '\n' || c == '\0' || c == '#')
            {
                if (c == '#')
                {
                    reader.ReadChar();
                    while (c != '\n')
                    {
                        try
                        {
                            c = reader.ReadChar();
                        }
                        catch (Exception)
                        {
                            reader.BaseStream.Seek(1, SeekOrigin.Current);
                        }
                    }
                }
                c = reader.ReadChar();
            }

            reader.BaseStream.Seek(-1, SeekOrigin.Current);
        }

        private static int ReadValue(BinaryReader reader)
        {
            StringBuilder builder = new StringBuilder();
            char c = reader.ReadChar();

            while (c != ' ' && c != '\t' && c != '\n' && c != '\0')
            {
                builder.Append(c);
                c = reader.ReadChar();
            }

            return int.Parse(builder.ToString());
        }

#endregion

        private async Task UpdateCanvas(byte[] data, int width, int height)
        {
            _bitmap = new WriteableBitmap(width, height);
           
            using (Stream bitmapStream = _bitmap.PixelBuffer.AsStream())
            {
                await bitmapStream.WriteAsync(data, 0, data.Length);
            }

            _actualImage = new ImageBrush
            {
                Stretch = Stretch.UniformToFill,
                ImageSource = _bitmap
            };

            _canvas.Background = _actualImage;
            _canvas.Width = width;
            _canvas.Height = height;
        }

        public void RevertToOriginalImage()
        {
            CheckForImageToLoad(_file);
        }

        private async void ShowErrorMessage(ImageLoadError error, Exception e = null)
        {
            var dialog = new MessageDialog("")
            {
                Title = "Błąd"
            };

            switch (error)
            {
                case ImageLoadError.WrongHeader:
                    break;
                case ImageLoadError.WrongSomething:
                    break;
                case ImageLoadError.Exception:
                    if (e != null)
                    {
                        dialog.Content = "Wystąpił wyjątek: " + e.Message;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }

            await dialog.ShowAsync();
        }

        private async Task ShowElapsedTime(long timeMilis)
        {
            if (timeMilis > 300)
            {
                var dialog = new MessageDialog("Czas przetwarzania: " + timeMilis / 1000 + "." + timeMilis % 1000);
                await dialog.ShowAsync();
            }
        }
    }
}
