using BlackJack.Domain.GameModels;

namespace BlackJackTests.UnitTests
{

    public class BlackjackTestData
    {
        public static IEnumerable<object[]> Hands =>
            new List<object[]>
            {
            new object[] { new Card[] { new Card(Suit.Hearts, Rank.Ace), new Card(Suit.Spades, Rank.King) }, 21 },
            };


    }
}
