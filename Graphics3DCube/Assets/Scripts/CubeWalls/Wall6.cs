using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CubeWalls
{
    class Wall6 : CubeBaseWall
    {
        protected override Sprite RgbPartSprite()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var color = new Color(j / Height, i / Width, 0);
                    Texture2D.SetPixel(i, j, color);
                }
            }
            return CreateSprite();
        }
    }
}
