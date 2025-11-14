using BlackJack.Game.GameModels;
using System.Reflection.Metadata.Ecma335;

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
            //Todo: does anything happen with the 3 or so cards, or are they just tossed?
            if(Deck.Cards.Count < 4)
            {
                CreateDeck();
            }

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
        /// </summary>
        /// <returns>Game state as player busted if over twenty one, otherwise in progress</returns>
        public GameState PlayerHit()
        {
            //check if round finished

            PlayerHand.AddCard(Deck.Draw());

            if(PlayerHand.GetValue() > 21) return GameState.PlayerBusted;

            return GameState.InProgress;
        }

        public GameState PlayerStand()
        {
            DealerPlays();

            if (DealerHand.GetValue() > 21) return GameState.DealerBusted;

            return DetermineWinner();
        }

        public void DealerPlays()
        {
            while (DealerHand.GetValue() < 17)
            {
                DealerHand.AddCard(Deck.Draw());
            }
        }

        public GameState DetermineWinner()
        {
            int playerValue = PlayerHand.GetValue();
            int dealerValue = DealerHand.GetValue();

            if (playerValue == dealerValue) return GameState.Push;

            return playerValue > dealerValue ? GameState.PlayerWin : GameState.DealerWin;
        }

    }
}
