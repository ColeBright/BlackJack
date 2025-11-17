using BlackJack.Game;
using BlackJack.Domain.GameModels;

namespace BlackJackTests.UnitTests
{
    public class GameEngineTests
    {
        [Fact]
        public void StartRound_DealsTwoCardsEach()
        {
            var engine = new GameEngine();
            var outcome = engine.StartRound();

            Assert.Equal(2, engine.PlayerHand.Cards.Count);
            Assert.Equal(2, engine.DealerHand.Cards.Count);
        }

        [Fact]
        public void DealtCardsAlternate()
        {
            // Arrange player and dealer cards in expected order

            var player1 = new Card(Suit.Hearts, Rank.Two);
            var dealer1 = new Card(Suit.Spades, Rank.Three);
            var player2 = new Card(Suit.Clubs, Rank.Four);
            var dealer2 = new Card(Suit.Diamonds, Rank.Five);

            //Arrange deck deterministically
            var deck = new Deck(new[] { player1, dealer1, player2, dealer2 });

            var engine = new GameEngine(deck);

            var result = engine.StartRound();

            Assert.Same(player1, engine.PlayerHand.Cards[0]);
            Assert.Same(player2, engine.PlayerHand.Cards[1]);
            Assert.Same(dealer1, engine.DealerHand.Cards[0]);
            Assert.Same(dealer2, engine.DealerHand.Cards[1]);
        }

        [Fact]
        public void PlayerHitsAndBusts()
        {
            // Arrange player and dealer cards in expected order

            var player1 = new Card(Suit.Hearts, Rank.Ten);
            var dealer1 = new Card(Suit.Spades, Rank.Three);
            var player2 = new Card(Suit.Clubs, Rank.Ten);
            var dealer2 = new Card(Suit.Diamonds, Rank.Five);
            var overTwentyOne = new Card(Suit.Diamonds, Rank.Two);

            //Arrange deck deterministically
            var deck = new Deck(new[] { player1, dealer1, player2, dealer2, overTwentyOne });

            var engine = new GameEngine(deck);

            engine.StartRound();

            var result = engine.PlayerHit();

            Assert.Equal(GameState.PlayerBusted, result);
        }

        [Fact]
        public void PlayerHitsAndDoesNotBust()
        {
            // Arrange player and dealer cards in expected order

            var player1 = new Card(Suit.Hearts, Rank.Ten);
            var dealer1 = new Card(Suit.Spades, Rank.Three);
            var player2 = new Card(Suit.Clubs, Rank.Eight);
            var dealer2 = new Card(Suit.Diamonds, Rank.Five);
            var overTwentyOne = new Card(Suit.Diamonds, Rank.Two);

            //Arrange deck deterministically
            var deck = new Deck(new[] { player1, dealer1, player2, dealer2, overTwentyOne });

            var engine = new GameEngine(deck);

            engine.StartRound();

            var result = engine.PlayerHit();

            Assert.Equal(GameState.InProgress, result);
        }

        [Fact]
        public void PlayerStandsAndDealerBusts()
        {
            // Arrange player and dealer cards in expected order

            var player1 = new Card(Suit.Hearts, Rank.Ten);
            var dealer1 = new Card(Suit.Spades, Rank.Ten);
            var player2 = new Card(Suit.Clubs, Rank.Eight);
            var dealer2 = new Card(Suit.Diamonds, Rank.Five);
            var overTwentyOne = new Card(Suit.Diamonds, Rank.Seven);

            //Arrange deck deterministically
            var deck = new Deck(new[] { player1, dealer1, player2, dealer2, overTwentyOne });

            var engine = new GameEngine(deck);

            engine.StartRound();

            var result = engine.PlayerStand();

            Assert.Equal(GameState.DealerBusted, result);
        }

        [Fact]
        public void PlayerStandsAndTieOccurs()
        {
            // Arrange player and dealer cards in expected order

            var player1 = new Card(Suit.Hearts, Rank.Ten);
            var dealer1 = new Card(Suit.Spades, Rank.Ten);
            var player2 = new Card(Suit.Clubs, Rank.Eight);
            var dealer2 = new Card(Suit.Diamonds, Rank.Eight);

            //Arrange deck deterministically
            var deck = new Deck(new[] { player1, dealer1, player2, dealer2 });

            var engine = new GameEngine(deck);

            engine.StartRound();

            var result = engine.PlayerStand();

            Assert.Equal(GameState.Push, result);
        }

        [Fact]
        public void PlayerStandsAndPlayerWins()
        {
            // Arrange player and dealer cards in expected order

            var player1 = new Card(Suit.Hearts, Rank.Ten);
            var dealer1 = new Card(Suit.Spades, Rank.Ten);
            var player2 = new Card(Suit.Clubs, Rank.Nine);
            var dealer2 = new Card(Suit.Diamonds, Rank.Eight);

            //Arrange deck deterministically
            var deck = new Deck(new[] { player1, dealer1, player2, dealer2 });

            var engine = new GameEngine(deck);

            engine.StartRound();

            var result = engine.PlayerStand();

            Assert.Equal(GameState.PlayerWin, result);
        }

        [Fact]
        public void PlayerStandsAndDealerWins()
        {
            // Arrange player and dealer cards in expected order

            var player1 = new Card(Suit.Hearts, Rank.Ten);
            var dealer1 = new Card(Suit.Spades, Rank.Ten);
            var player2 = new Card(Suit.Clubs, Rank.Eight);
            var dealer2 = new Card(Suit.Diamonds, Rank.Nine);

            //Arrange deck deterministically
            var deck = new Deck(new[] { player1, dealer1, player2, dealer2 });

            var engine = new GameEngine(deck);

            engine.StartRound();

            var result = engine.PlayerStand();

            Assert.Equal(GameState.DealerWin, result);
        }
    }
}
