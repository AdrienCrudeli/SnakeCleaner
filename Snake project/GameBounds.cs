using System;

namespace Snake
{
    internal sealed class GameBounds
    {
        private GameBounds(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public static GameBounds Create(int targetWidth, int targetHeight)
        {
            int width = targetWidth;
            int height = targetHeight;

            if (OperatingSystem.IsWindows())
            {
                Console.WindowHeight = targetHeight;
                Console.WindowWidth = targetWidth;
                width = Console.WindowWidth;
                height = Console.WindowHeight;
            }
            else
            {
                width = Math.Min(Console.WindowWidth, targetWidth);
                height = Math.Min(Console.WindowHeight, targetHeight);
            }

            return new GameBounds(width, height);
        }
    }
}
