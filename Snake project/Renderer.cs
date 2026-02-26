using System;
using System.Collections.Generic;

namespace Snake
{
    internal sealed class Renderer
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void DrawBorders(GameBounds bounds)
        {
            for (int x = 0; x < bounds.Width; x++)
            {
                DrawPixel(x, 0);
            }

            for (int x = 0; x < bounds.Width; x++)
            {
                DrawPixel(x, bounds.Height - 1);
            }

            for (int y = 0; y < bounds.Height; y++)
            {
                DrawPixel(0, y);
            }

            for (int y = 0; y < bounds.Height; y++)
            {
                DrawPixel(bounds.Width - 1, y);
            }
        }

        public void DrawTail(IReadOnlyList<Position> tail)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i < tail.Count; i++)
            {
                DrawPixel(tail[i].X, tail[i].Y);
            }
        }

        public void DrawHeadAndBerry(SnakeEntity snake, Berry berry)
        {
            Console.SetCursorPosition(snake.HeadX, snake.HeadY);
            Console.ForegroundColor = snake.Color;
            Console.Write("■");

            Console.SetCursorPosition(berry.X, berry.Y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        public void DrawGameOver(GameBounds bounds, int score)
        {
            Console.ResetColor();
            Console.SetCursorPosition(bounds.Width / 5, bounds.Height / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(bounds.Width / 5, bounds.Height / 2 + 1);
        }

        private static void DrawPixel(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("■");
        }
    }
}
