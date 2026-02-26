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

                if (TryGetDirectionFromKey(key, out Direction requestedDirection) &&
                    !IsOpposite(requestedDirection, snake.Direction))
                {
                    snake.Direction = requestedDirection;
                    buttonPressed = true;
                }
            }
        }

        private static bool TryGetDirectionFromKey(ConsoleKey key, out Direction direction)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    direction = Direction.Up;
                    return true;
                case ConsoleKey.DownArrow:
                    direction = Direction.Down;
                    return true;
                case ConsoleKey.LeftArrow:
                    direction = Direction.Left;
                    return true;
                case ConsoleKey.RightArrow:
                    direction = Direction.Right;
                    return true;
                default:
                    direction = default;
                    return false;
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
