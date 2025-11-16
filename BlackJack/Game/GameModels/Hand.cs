namespace BlackJack.Game.GameModels
{
    public class Hand
    {
        private readonly List<Card> _cards;

        public bool IsDealer { get; }

        public Hand(bool isDealer = false) {
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

            while(total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }

            return total;
        }

        public bool IsBlackjack()
        {
            return _cards.Count() == 2 && _cards.Sum(c => c.GetValue()) == 21;
        }

    }
}