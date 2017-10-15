using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes
{
    public class CurrentShape
    {
        private Shape _currentShape;
        private CompositeTransform _currentCompositeTransform;
        private readonly Random _random;

        public bool IsDragging;

        public CurrentShape()
        {
            _random = new Random();
        }

        public void Create(Shape shape, Point position)
        {
            AssignEventsToShapeObject(shape);
            shape.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255)));

            shape.Width = 30;
            shape.Height = 30;

            Canvas.SetLeft(shape, position.X);
            Canvas.SetTop(shape, position.Y);
        }

        private void OnMouseDown(object sender, PointerRoutedEventArgs e)
        {
            IsDragging = true;
        }

        private void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Debug.WriteLine("Ellipse manipulation started");
            _currentShape = (Shape)sender;
            _currentCompositeTransform = _currentShape.RenderTransform as CompositeTransform;
            this._currentShape.Opacity = 0.4;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _currentCompositeTransform.TranslateX += e.Delta.Translation.X;
            _currentCompositeTransform.TranslateY += e.Delta.Translation.Y;
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Debug.WriteLine("Manipulation of ellipse finished");
            this._currentShape.Opacity = 1;
        }

        private void OnMouseUp(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Object MouseUp");
            IsDragging = false;
        }

        private void AssignEventsToShapeObject(Shape shape)
        {
            shape.RenderTransform = new CompositeTransform();

            shape.PointerPressed += OnMouseDown;
            shape.PointerReleased += OnMouseUp;

            shape.ManipulationMode =
                ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.Scale;
            shape.ManipulationStarted += OnManipulationStarted;
            shape.ManipulationDelta += OnManipulationDelta;
            shape.ManipulationCompleted += OnManipulationCompleted;
        }
    }
}
