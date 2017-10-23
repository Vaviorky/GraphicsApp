﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private readonly Canvas _canvas;

        public ImageManager(Canvas canvas)
        {
            _canvas = canvas;
        }

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

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                var fileType = file.FileType;
                if (fileType.Equals(".jpg") || fileType.Equals(".jpeg"))
                {
                    await LoadJpg(file);
                }
                else if (fileType.Equals(".ppm"))
                {
                    // Create new stopwatch.
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    await LoadPpm(file);
                    stopwatch.Stop();
                    Debug.WriteLine("Elapsed Time: " + stopwatch.ElapsedMilliseconds / 1000 + "." + stopwatch.ElapsedMilliseconds % 1000);
                }
            }
        }

        public async Task Save()
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(_canvas);

            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Obraz JPEG", new[] { ".jpg" });
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                var pixels = await renderTargetBitmap.GetPixelsAsync();

                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                    var bytes = pixels.ToArray();
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                        (uint)_canvas.ActualWidth, (uint)_canvas.ActualHeight, 96, 96, bytes);

                    await encoder.FlushAsync();
                }
            }
        }

        private async Task LoadJpg(IStorageFile file)
        {
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                var displayableImage = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);

                var sbs = new SoftwareBitmapSource();
                await sbs.SetBitmapAsync(displayableImage);

                var source = new ImageBrush { ImageSource = sbs };

                _canvas.Background = source;
            }
        }


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
                    //wrong file format
                }
            }
        }

        private async Task LoadP3(StreamReader reader)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            reader.ReadLine();

            var parameters = LoadParameters(reader);

            Debug.WriteLine("1: " + watch.ElapsedMilliseconds);

            if (parameters.Length == 0) return;

            int width = parameters[0];
            int height = parameters[1];
            int max = parameters[2];

            Debug.WriteLine("2: " + watch.ElapsedMilliseconds);

            int[] rawData = await ReadImgInformation(reader, parameters[0], parameters[1]);
            Debug.WriteLine("3: " + watch.ElapsedMilliseconds);
            byte[] preparedData = LoadP3Img(width, height, max, rawData);
            Debug.WriteLine("4: " + watch.ElapsedMilliseconds);
            var actualImage = new ImageBrush
            {
                ImageSource = await ImageFromBytes(preparedData, width, height)
            };
            Debug.WriteLine("5: " + watch.ElapsedMilliseconds);
            _canvas.Background = actualImage;
            watch.Stop();
            Debug.WriteLine("6: " + watch.ElapsedMilliseconds);

        }

        private int[] LoadParameters(StreamReader reader)
        {
            int width = -1;
            int height = -1;
            int max = -1;

            do
            {
                var line = reader.ReadLine().Replace("\t", " ");
                var words = line.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    var word = words[i];
                    if (word.Contains("#"))
                    {
                        break;
                    }

                    if (word == "" || word.Contains(" ") || word.Contains("\t") || word.Contains("\0") ||
                        word.Contains("\n"))
                    {
                        continue;
                    }

                    try
                    {
                        var number = int.Parse(word);
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
                        }
                    }
                    catch (Exception e)
                    {
                        ShowErrorMessage(ImageLoadError.Exception, e);
                        return new int[] { };
                    }
                }
            } while (max == -1);

            return new[] { width, height, max };
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
                Debug.WriteLine(e);
                throw;
            }
            return array;
        }

        private int counter = 0;

        private async Task<int[]> ReadImgInformation(StreamReader reader, int width, int height)
        {
            int[] data = new int[width * height * 3];
            int counter = 0;

            var splitter = new FastSplit(1);

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

        private int temp;
        private int ParseInt(string str)
        {
            temp = 0;
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

        private bool IsHashInString(string str)
        {
            for (int j = 0; j < str.Length; j++)
            {
                if (str[j] == '#')
                {
                    return true;
                }
            }
            return false;
        }



        private async Task<WriteableBitmap> ImageFromBytes(byte[] data, int width, int height)
        {
            WriteableBitmap bitmap = new WriteableBitmap(width, height);
            using (Stream bitmapStream = bitmap.PixelBuffer.AsStream())
            {
                await bitmapStream.WriteAsync(data, 0, data.Length);
            }

            return bitmap;
        }

        private void ShowErrorMessage(ImageLoadError error, Exception e = null)
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

            dialog.ShowAsync();
        }
    }
}
