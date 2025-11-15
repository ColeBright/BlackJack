using BlackJack.Game.GameModels;
using BlackJack.Helpers;

namespace BlackJack.Game
{
    public enum GameState
    {
        InProgress,
        PlayerBusted,
        DealerBusted,
        PlayerBlackjack,
        DealerBlackjack,
        PlayerWin,
        DealerWin,
        Push
    }
    public class GameEngine
    {
        public Deck Deck { get; private set; }

        public Hand PlayerHand { get; private set; }
        public Hand DealerHand { get; private set; }

        public GameEngine() 
        { 
            CreateDeck();
            RefreshHands();
        }

        public GameEngine(Deck deck)
        {
            Deck = deck;
            RefreshHands();
        }

        public void CreateDeck()
        {
            Deck = new Deck();
            Deck.Shuffle();
        }

        public void RefreshHands()
        {
            PlayerHand = new Hand();
            DealerHand = new Hand(isDealer: true);
        }

        /// <summary>
        ///  Starts a new round
        ///  Creates and shuffles a new deck if there is not enough to deal
        ///  checks for initial blackjack or blackjack ties
        /// </summary>
        /// <returns>The resulting GameState enum</returns>
        public GameState StartRound()
        {
            // so cards don't run out mid-deal
            EnsureCardsAvailable(4);

            RefreshHands();

            //Deal
            PlayerHand.AddCard(Deck.Draw());
            DealerHand.AddCard(Deck.Draw());
            PlayerHand.AddCard(Deck.Draw());
            DealerHand.AddCard(Deck.Draw());

            //Check for naturals
            bool playerBlackJack = PlayerHand.GetValue() == 21;
            bool dealerBlackJack = DealerHand.GetValue() == 21;

            if (playerBlackJack && dealerBlackJack) return GameState.Push;

            if (playerBlackJack) return GameState.PlayerBlackjack;
            if (dealerBlackJack) return GameState.DealerBlackjack;

            return GameState.InProgress;
        }

        /// <summary>
        /// Player requests additional card, checks the new total
        /// Throws exception if round is not in progress
        /// </summary>
        /// <returns>Game state as player busted if over twenty one, otherwise in progress</returns>
        public GameState PlayerHit()
        {

            if (IsRoundFinished()) throw new InvalidOperationException("Round already finished.");

            EnsureCardsAvailable();

            PlayerHand.AddCard(Deck.Draw());

            if(PlayerHand.GetValue() > 21) return GameState.PlayerBusted;

            return GameState.InProgress;
        }

        /// <summary>
        /// Player decides to stay, triggering dealer to make final move
        /// Throws exception if round is not in progress
        /// </summary>
        /// <returns>Game state as dealer busted if over twenty one, otherwise in pass to DetermineWinner</returns>
        public GameState PlayerStand()
        {
            if (IsRoundFinished()) throw new InvalidOperationException("Round already finished.");

            DealerPlays();

            if (DealerHand.GetValue() > 21) return GameState.DealerBusted;

            return DetermineWinner();
        }

        public void DealerPlays()
        {
            while (DealerHand.GetValue() < 17)
            {
                EnsureCardsAvailable(1);
                DealerHand.AddCard(Deck.Draw());
            }
        }
        /// <summary>
        /// Checks the dealer and player values against each other and sets the resulting game state
        /// </summary>
        /// <returns>Resulting state of the game</returns>
        public GameState DetermineWinner()
        {
            int playerValue = PlayerHand.GetValue();
            int dealerValue = DealerHand.GetValue();

            if (playerValue == dealerValue) return GameState.Push;

            return playerValue > dealerValue ? GameState.PlayerWin : GameState.DealerWin;
        }
        
        public bool IsRoundFinished()
        {
            var outcome = PeekState();
            return outcome != GameState.InProgress;
        }

        public GameState PeekState()
        {
            int playerValue = PlayerHand.GetValue();

            if (playerValue > 21) return GameState.PlayerBusted;

            return GameState.InProgress;
        }

        private void EnsureCardsAvailable(int requiredAmount = 1)
        {
            if (Deck == null) CreateDeck();

            if (Deck.Cards.Count < requiredAmount)
            {
                //could decide what to do with remaining amount here
                CreateDeck();
            }
        }

        public void LoadHands(IEnumerable<string> playerCardKeys, IEnumerable<string> dealerCardKeys)
        {
            if (playerCardKeys == null) playerCardKeys = Array.Empty<string>();
            if (dealerCardKeys == null) dealerCardKeys = Array.Empty<string>();

            var newPlayer = new Hand();
            foreach (var k in playerCardKeys)
            {
                var card = CardSerialization.FromKey(k);
                newPlayer.AddCard(card);
            }

            var newDealer = new Hand(isDealer: true);
            foreach (var k in dealerCardKeys)
            {
                var card = CardSerialization.FromKey(k);
                newDealer.AddCard(card);
            }

            // Atomically replace the hands
            PlayerHand = newPlayer;
            DealerHand = newDealer;
        }

    }
}
