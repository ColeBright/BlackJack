
namespace BlackJack.Domain.GameModels
{
    public class Hand
    {
        private readonly List<Card> _cards;

        public bool IsDealer { get; }

        public Hand(bool isDealer = false)
        {
            IsDealer = isDealer;
            _cards = new List<Card>();
        }

        public Hand(IEnumerable<Card> cards, bool isDealer = false)
        {
            if (cards == null) throw new ArgumentNullException(nameof(cards));
            IsDealer = isDealer;
            _cards = new List<Card>(cards);
        }

        public IReadOnlyList<Card> Cards => _cards;
        public void AddCard(Card card) => _cards.Add(card);

        public int GetValue()
        {
            int total = _cards.Sum(c => c.GetValue());
            int aces = _cards.Count(c => c.Rank == Rank.Ace);

            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }

            return total;
        }

        public bool IsBlackjack()
        {
            // Natural blackjack: exactly 2 cards - one Ace and one 10-value card (10, Jack, Queen, or King)
            if (_cards.Count != 2) return false;
            
            bool hasAce = _cards.Any(c => c.Rank == Rank.Ace);
            bool hasTenValue = _cards.Any(c => c.Rank == Rank.Ten || c.Rank == Rank.Jack || 
                                               c.Rank == Rank.Queen || c.Rank == Rank.King);
            
            return hasAce && hasTenValue && GetValue() == 21;
        }

    }
}
