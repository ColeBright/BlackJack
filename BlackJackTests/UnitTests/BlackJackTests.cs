using BlackJack.Domain.GameModels;

namespace BlackJackTests.UnitTests
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

        [Theory]
        [InlineData(GameState.PlayerBlackjack, 100, 50, 275)]
        [InlineData(GameState.PlayerWin, 100, 50, 200)]
        [InlineData(GameState.DealerBusted, 100, 50, 200)]
        [InlineData(GameState.Push, 100, 50, 150)]
        public void BetCalculatesCorrectlyGivenGameState(GameState state, decimal playerBalance, decimal bet, decimal expected) 
        {
            var betTest = new Bet(bet, playerBalance, true);

            betTest.CalculateWinnings(state);

            Assert.Equal(betTest.PlayerBalance, expected);
        }

        public void PlaceBetZeroThrowsException()
        {
            var 
            Assert.Throws<ArgumentOutOfRangeException>(() => {
        }
    }
}
