using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicsProject.Classes.ImageProcessing
{
    static class Mask
    {
        public static int[,] Mean => new int[,]
        {
            {1, 1, 1},
            {1, 1, 1},
            {1, 1, 1},
        };

        public static int[,] Sharp => new int[,]
        {
            {0, -1, 0},
            {-1, 5, -1},
            {0, -1, 0},
        };

        public static int[,] Sobel => new int[,]
        {
            {1, 2, 1},
            {0, 0, 0},
            {-1, -2, -1},
        };

        public static int[,] Gauss => new int[,]
        {
            {1, 2, 1},
            {2, 4, 2},
            {1, 2, 1},
        };
    }
}
