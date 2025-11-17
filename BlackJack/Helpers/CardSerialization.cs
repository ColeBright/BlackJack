using BlackJack.Domain.GameModels;

namespace BlackJack.Helpers
{
    public static class CardSerialization
    {
        public static string ToKey(Card c) => $"{c.Suit}|{c.Rank}";

        public static Card FromKey(string key)
        {
            var parts = key.Split('|');
            var suit = Enum.Parse<Suit>(parts[0]);
            var rank = Enum.Parse<Rank>(parts[1]);

            return new Card(suit, rank);
        }
    }
}
