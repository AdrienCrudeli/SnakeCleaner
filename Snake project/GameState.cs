namespace Snake
{
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
}
