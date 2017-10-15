using GraphicsProject.Classes;
using GraphicsProject.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace GraphicsProject.Views
{
    public sealed partial class Main : Page
    {
        private bool _isDrawing;
        private readonly ShapeType _shapeType;
        private readonly ShapeManager _shapeManager;

        public Main()
        {
            this._shapeType = ShapeType.Rectangle;
            _isDrawing = false;
            this.InitializeComponent();
            this._shapeManager = new ShapeManager(DrawingCanvas);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void Canvas_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _isDrawing = true;
            Debug.WriteLine("Canvas Mouse Pressed");
            var point = e.GetCurrentPoint(DrawingCanvas);
            _shapeManager.CreateNewShape(_shapeType, point.Position);
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
    }
}
