using System;
using System.Collections.Generic;

namespace Snake
{
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
}
