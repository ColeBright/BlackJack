


namespace BlackJack.Domain.GameModels
{
    public class Bet
    {
        public decimal PlayerBalance { get; set; } = 0;
        public decimal CurrentBet { get; set; } = 0;

        public Bet(decimal playerbalance)
        {
            PlayerBalance = playerbalance;
        }

        public void PlaceBet(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Bet must be positive.");
            if (amount > PlayerBalance) throw new InvalidOperationException("Insufficient balance to place that bet");

            CurrentBet = amount;
        }

        public void CalculateWinnings(GameState? gameState)
        {
            switch (gameState)
            {
                case GameState.PlayerWin:
                case GameState.DealerBusted:
                    PlayerBalance += CurrentBet;
                    break;
                case GameState.PlayerBlackjack:
                    PlayerBalance += CurrentBet * 1.5m;
                    break;
                case GameState.PlayerBusted:
                case GameState.DealerWin:
                case GameState.DealerBlackjack:
                    PlayerBalance += CurrentBet;
                    break;
            }

            CurrentBet = 0;
        }
    }
}
