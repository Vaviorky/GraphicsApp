using System;
using UnityEngine;

namespace Assets.Scripts.CubeWalls
{
    public class Wall1 : CubeBaseWall
    {
        protected override Sprite RgbPartSprite()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var color = new Color(i / Height, 0, j / Width);
                    Texture2D.SetPixel(i, j, color);
                }
            }
            return CreateSprite();
        }
    }
}

