﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace GraphicsProject.Classes.Shapes
{
    internal static class ShapeDrawer
    {
        public static void InitializeRectangle(Rectangle rectangle, Point startingPosition, Point endingPosition)
        {
            var x = startingPosition.X < endingPosition.X ? startingPosition.X : endingPosition.X;
            var y = startingPosition.Y < endingPosition.Y ? startingPosition.Y : endingPosition.Y;

            rectangle.Width = Math.Abs(endingPosition.X - startingPosition.X);
            rectangle.Height = Math.Abs(endingPosition.Y - startingPosition.Y);

            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);
        }

        public static void InitializeCircle(Ellipse ellipse, Point startingPosition, Point endingPosition)
        {
            var radius = Math.Sqrt(Math.Pow(endingPosition.X - startingPosition.X, 2) + 
                Math.Pow(endingPosition.Y - startingPosition.Y, 2));

            var size = 2 * radius;

            ellipse.Width = size;
            ellipse.Height = size;

            Canvas.SetLeft(ellipse, startingPosition.X - radius);
            Canvas.SetTop(ellipse, startingPosition.Y - radius);
        }

        public static void InitializeLine(Line line, Point startingPosition, Point endingPosition)
        {
            var width = startingPosition.X > endingPosition.X ? startingPosition.X : endingPosition.X;
            var height = startingPosition.Y > endingPosition.Y ? startingPosition.Y : endingPosition.Y;

            Canvas.SetLeft(line, 0);
            Canvas.SetTop(line, 0);

            line.X1 = startingPosition.X;
            line.Y1 = startingPosition.Y;

            line.X2 = endingPosition.X;
            line.Y2 = endingPosition.Y;

            line.Width = width;
            line.Height = height;
        }

        private static void AddSpecialPointsToLine(Line line, Point startingPoint, Point endingPoint)
        {

        }
    }
}
