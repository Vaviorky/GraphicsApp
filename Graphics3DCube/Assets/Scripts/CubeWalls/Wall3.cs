using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CubeWalls
{
    class Wall3 : CubeBaseWall
    {
        protected override Sprite RgbPartSprite()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = (int)Height; j >= 0; j--)
                {
                    var color = new Color(j / Height, 1, i / Width);
                    Texture2D.SetPixel(i, j, color);
                }
            }

            return CreateSprite();
        }
    }
}
