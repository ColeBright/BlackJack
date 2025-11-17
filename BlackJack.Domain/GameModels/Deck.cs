
namespace BlackJack.Domain.GameModels
{
    public class Deck
    {
        //52 card deck

        public List<Card> Cards { get; set; } = new List<Card>();

        public Random randomNumber = new Random();

        public Deck()
        {

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    Cards.Add(new Card(suit, rank));
                }
            }
        }

        public Deck(IEnumerable<Card> cards)
        {
            Cards = cards.ToList();
        }

        public void Shuffle()
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                int j = randomNumber.Next(Cards.Count);
                Card temp = Cards[i];
                Cards[i] = Cards[j];
                Cards[j] = temp;
            }
        }

        public Card Draw()
        {
            if (!Cards.Any()) throw new InvalidOperationException("No cards in deck");
            var card = Cards[0];
            Cards.Remove(card);
            return card;
        }
    }
}
