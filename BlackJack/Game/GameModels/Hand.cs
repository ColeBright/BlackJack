namespace BlackJack.Game.GameModels
{
    public class Hand
    {
        private List<Card> cards = new List<Card>();

        public bool IsDealer { get; }

        public Hand(bool isDealer = false) {
            IsDealer = isDealer;
        }

        public IReadOnlyList<Card> Cards => cards;
        public void AddCard(Card card) => cards.Add(card);

        public int GetValue()
        {
            int total = cards.Sum(c => c.GetValue());
            int aces = cards.Count(c => c.Rank == Rank.Ace);

            while(total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }

            return total;
        }

    }
}