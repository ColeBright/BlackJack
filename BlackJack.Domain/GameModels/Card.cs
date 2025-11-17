using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Domain.GameModels
{
    public enum Suit { Hearts, Diamonds, Clubs, Spades }
    public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }
    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public int GetValue()
        {
            // If number card, return number value
            if (Rank >= Rank.Two && Rank <= Rank.Ten) return (int)Rank;

            // If face card return 10
            if (Rank >= Rank.Jack && Rank <= Rank.King) return 10;

            // If Ace, default to 11
            return 11;
        }

        public override string ToString() => $"{Rank} of {Suit}";

    }
}
