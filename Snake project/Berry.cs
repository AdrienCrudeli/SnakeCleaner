using System;

namespace Snake
{
    internal readonly struct Berry
    {
        public Berry(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public static Berry SpawnAnywhere(Random random, GameBounds bounds)
        {
            return new Berry(random.Next(0, bounds.Width), random.Next(0, bounds.Height));
        }

        public static Berry SpawnInsideBorder(Random random, GameBounds bounds)
        {
            return new Berry(
                random.Next(1, bounds.Width - 2),
                random.Next(1, bounds.Height - 2));
        }
    }
}
