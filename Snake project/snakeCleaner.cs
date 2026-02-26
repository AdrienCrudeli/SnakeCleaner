// Source - https://codereview.stackexchange.com/q/127515
// Posted by Wagacca, modified by community. See post 'Timeline' for change history
// Retrieved 2026-02-12, License - CC BY-SA 3.0

using System;

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
}
