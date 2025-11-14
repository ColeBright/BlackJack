using BlackJack.Game.GameModels;

namespace BlackJackTests
{
    public class BlackJackTests
    {
        [Fact]
        public void DeckHas52Cards()
        {
            var deck = new Deck();
            Assert.Equal(52, deck.Cards.Count());
        }


        [Theory]
        [MemberData(nameof(BlackjackTestData.Hands), MemberType = typeof(BlackjackTestData))]
        public void HandTotalCalculatesCorrectly(Card[] cards, int expectedValue)
        {
            var hand = new Hand();
            foreach (var card in cards)
            {
                hand.AddCard(card);
            }

            Assert.Equal(expectedValue, hand.GetValue());

        }

        [Fact]
        public void AceEquals1WhenOver()
        {
            var hand = new Hand();
            hand.AddCard(new Card(Suit.Hearts, Rank.Ace));
            hand.AddCard(new Card(Suit.Spades, Rank.King));
            hand.AddCard(new Card(Suit.Diamonds, Rank.Queen));

            Assert.Equal(21, hand.GetValue());
        }
    }
}
