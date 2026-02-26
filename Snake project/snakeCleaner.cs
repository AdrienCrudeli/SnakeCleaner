// Source - https://codereview.stackexchange.com/q/127515
// Posted by Wagacca, modified by community. See post 'Timeline' for change history
// Retrieved 2026-02-12, License - CC BY-SA 3.0

using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    internal static class Program
    {
        private const int InitialScreenWidth = 32;
        private const int InitialScreenHeight = 16;
        private const int InitialScore = 5;
        private const int TickDurationMs = 500;

        private static void Main(string[] args)
        {
            GameBounds bounds = GameBounds.Create(InitialScreenWidth, InitialScreenHeight);
            Random random = new Random();

            SnakeEntity snake = SnakeEntity.CreateCentered(bounds, ConsoleColor.Red, Direction.Right);
            Berry berry = Berry.SpawnAnywhere(random, bounds);
            GameState gameState = new GameState(InitialScore);

            Renderer renderer = new Renderer();
            InputHandler inputHandler = new InputHandler();

            while (!gameState.IsGameOver)
            {
                renderer.Clear();

                if (snake.IsTouchingBorder(bounds))
                {
                    gameState.IsGameOver = true;
                }

                renderer.DrawBorders(bounds);

                if (snake.HeadX == berry.X && snake.HeadY == berry.Y)
                {
                    gameState.Score++;
                    berry = Berry.SpawnInsideBorder(random, bounds);
                }

                renderer.DrawTail(snake.Tail);

                if (snake.IsTouchingTail())
                {
                    gameState.IsGameOver = true;
                }

                if (gameState.IsGameOver)
                {
                    break;
                }

                renderer.DrawHeadAndBerry(snake, berry);
                inputHandler.CaptureDirectionForTick(snake, TickDurationMs);

                snake.AddCurrentHeadToTail();
                snake.MoveForward();
                snake.TrimTailToLength(gameState.Score);
            }

            renderer.DrawGameOver(bounds, gameState.Score);
        }
    }

    internal sealed class GameState
    {
        public GameState(int initialScore)
        {
            Score = initialScore;
            IsGameOver = false;
        }

        public int Score { get; set; }

        public bool IsGameOver { get; set; }
    }

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

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    internal readonly struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }

    internal sealed class SnakeEntity
    {
        private readonly List<Position> _tail = new List<Position>();

        private SnakeEntity(int headX, int headY, ConsoleColor color, Direction direction)
        {
            HeadX = headX;
            HeadY = headY;
            Color = color;
            Direction = direction;
        }

        public int HeadX { get; private set; }

        public int HeadY { get; private set; }

        public ConsoleColor Color { get; }

        public Direction Direction { get; set; }

        public IReadOnlyList<Position> Tail => _tail;

        public static SnakeEntity CreateCentered(GameBounds bounds, ConsoleColor color, Direction direction)
        {
            return new SnakeEntity(bounds.Width / 2, bounds.Height / 2, color, direction);
        }

        public void AddCurrentHeadToTail()
        {
            _tail.Add(new Position(HeadX, HeadY));
        }

        public void MoveForward()
        {
            switch (Direction)
            {
                case Direction.Up:
                    HeadY--;
                    break;
                case Direction.Down:
                    HeadY++;
                    break;
                case Direction.Left:
                    HeadX--;
                    break;
                case Direction.Right:
                    HeadX++;
                    break;
            }
        }

        public void TrimTailToLength(int maxLength)
        {
            if (_tail.Count > maxLength)
            {
                _tail.RemoveAt(0);
            }
        }

        public bool IsTouchingBorder(GameBounds bounds)
        {
            return HeadX == 0 ||
                   HeadY == 0 ||
                   HeadX == bounds.Width - 1 ||
                   HeadY == bounds.Height - 1;
        }

        public bool IsTouchingTail()
        {
            for (int i = 0; i < _tail.Count; i++)
            {
                if (_tail[i].X == HeadX && _tail[i].Y == HeadY)
                {
                    return true;
                }
            }

            return false;
        }
    }

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
