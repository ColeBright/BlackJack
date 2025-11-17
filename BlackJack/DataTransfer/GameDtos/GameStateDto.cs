using BlackJack.Web.DataTransfer.GameDtos;

namespace BlackJack.DataTransfer.GameDtos
{
    public class GameStateDto
    {
        public List<string> DeckKeys { get; set; } = new();
        public HandDto PlayerHand { get; set; } = new();
        public HandDto DealerHand { get; set; } = new();
        public bool PlayerHasStood { get; set; } = false;

        public BetDto Bet { get; set; } = new();
    }
}
