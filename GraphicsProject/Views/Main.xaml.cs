﻿using GraphicsProject.Classes;
using GraphicsProject.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using GraphicsProject.Classes.Shapes;

namespace GraphicsProject.Views
{
    public sealed partial class Main : Page
    {
        private bool _isDrawing;
        private ShapeType _shapeType;
        private readonly ShapeManager _shapeManager;
        private Color _currentColor;

        public Main()
        {
            this.InitializeComponent();
            _currentColor = ColorPicker.Color;
            this._shapeType = ShapeType.Rectangle;
            ButtonRectangleSelect.Background = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
            _isDrawing = false;
            this._shapeManager = new ShapeManager(DrawingCanvas);
            _shapeManager.OnShapeParameterChange += UpdateTextFields;
        }

        private void UpdateTextFields(Shape shape)
        {
            var cs = shape.TransformToVisual(DrawingCanvas);
            Point point = cs.TransformPoint(new Point(shape.Width / 2, shape.Height / 2));
            XPosition.Text = point.X.ToString(CultureInfo.InvariantCulture);
            YPosition.Text = point.Y.ToString(CultureInfo.InvariantCulture);
            Width.Text = shape.Width.ToString(CultureInfo.InvariantCulture);
            Height.Text = shape.Height.ToString(CultureInfo.InvariantCulture);
            ColorPicker.Color = ((SolidColorBrush)shape.Fill).Color;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void Canvas_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _isDrawing = true;
            Debug.WriteLine("Canvas Mouse Pressed, ShapeType: " + _shapeType);
            var point = e.GetCurrentPoint(DrawingCanvas);
            _shapeManager.CreateNewShape(_shapeType, point.Position, ColorPicker.Color);
        }

        private void Canvas_OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isDrawing = false;
            Debug.WriteLine("Canvas mouse up");
        }

        private void Canvas_OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!_isDrawing) return;

            if (_shapeManager.CanModifyShape())
            {
                _shapeManager.ModifyCreatedShape(e.GetCurrentPoint(DrawingCanvas).Position);
            }
        }

        private void Colorpick_ColorChanged(object sender, Color color)
        {
            _shapeManager.ChangeColorOfSelectedShape(color);
        }

        private void DrawingCanvas_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            Debug.WriteLine("asodijasoidjasiodjsioadioadioj");
        }

        private void XPosition_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Key.ToString().Contains("Number"))
            {
                e.Handled = true;
                return;
            }
            e.Handled = false;
        }

        private void Button_OnRectangleSelect(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("OnRectangleSelect");
            _shapeType = ShapeType.Rectangle;
        }

        private void Button_OnEllipseSelect(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("OnEllipseSelect");
            _shapeType = ShapeType.Circle;
        }

        private void Button_OnLineSelect(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnLineSelect");
            ButtonLineSelect.Background = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));
            _shapeType = ShapeType.Line;
        }

        private void Button_OnRectangleSelect(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnLineSelect");

            _shapeType = ShapeType.Rectangle;
        }

        private void Button_OnEllipseSelect(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnLineSelect");
            _shapeType = ShapeType.Circle;
        }
    }
}
