using System;
using System.Threading;

namespace Snake
{
    internal sealed class InputHandler
    {
        public void CaptureDirectionForTick(SnakeEntity snake, int tickDurationMs)
        {
            DateTime tickStart = DateTime.Now;
            bool buttonPressed = false;

            while (true)
            {
                DateTime now = DateTime.Now;

                if (now.Subtract(tickStart).TotalMilliseconds > tickDurationMs)
                {
                    break;
                }

                if (!Console.KeyAvailable)
                {
                    Thread.Sleep(1);
                    continue;
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                if (buttonPressed)
                {
                    continue;
                }

                Direction? requestedDirection = GetDirectionFromKey(key);

                if (requestedDirection.HasValue && !IsOpposite(requestedDirection.Value, snake.Direction))
                {
                    snake.Direction = requestedDirection.Value;
                    buttonPressed = true;
                }
            }
        }

        private static Direction? GetDirectionFromKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return Direction.Up;
                case ConsoleKey.DownArrow:
                    return Direction.Down;
                case ConsoleKey.LeftArrow:
                    return Direction.Left;
                case ConsoleKey.RightArrow:
                    return Direction.Right;
                default:
                    return null;
            }
        }

        private static bool IsOpposite(Direction a, Direction b)
        {
            return (a == Direction.Up && b == Direction.Down) ||
                   (a == Direction.Down && b == Direction.Up) ||
                   (a == Direction.Left && b == Direction.Right) ||
                   (a == Direction.Right && b == Direction.Left);
        }
    }
}
