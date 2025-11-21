


namespace BlackJack.Domain.GameModels
{
    public class Bet
    {
        public decimal PlayerBalance { get; set; } = 0;
        public decimal CurrentBet { get; set; } = 0;

        public bool IsActive { get; private set; } = false;

        public Bet(decimal playerbalance)
        {
            PlayerBalance = playerbalance;
        }
        public Bet(decimal currentBet, decimal playerbalance, bool isActive)
        {
            PlayerBalance = playerbalance;
            CurrentBet = currentBet;
            IsActive = isActive;
        }

        public void PlaceBet(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Bet must be positive.");
            if (amount > PlayerBalance) throw new InvalidOperationException("Insufficient balance to place that bet");

            IsActive = true;
            CurrentBet = amount;
            PlayerBalance -= amount;
        }

        public void CalculateWinnings(GameState? gameState)
        {
            if (this.IsActive)
            {
                switch (gameState)
                {
                    case GameState.PlayerWin:
                    case GameState.DealerBusted:
                        PlayerBalance += CurrentBet * 2;
                        break;
                    case GameState.PlayerBlackjack:
                        PlayerBalance += (CurrentBet * 1.5m) + (CurrentBet * 2);
                        break;
                    case GameState.Push:
                        PlayerBalance += CurrentBet;
                        break;
                }
                CurrentBet = 0;
                IsActive = false;
            }
        }
    }
}
