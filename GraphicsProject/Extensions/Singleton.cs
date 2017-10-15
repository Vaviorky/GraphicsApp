﻿namespace GraphicsProject.Extensions
{
    public abstract class Singleton<T>where T : Singleton<T>, new()
    {
        public static T Instance { get; } = new T();
    }
}
