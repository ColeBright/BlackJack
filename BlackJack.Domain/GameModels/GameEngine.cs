using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Domain.GameModels
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
        public bool PlayerHasStood { get; private set; }

        public Bet PlayerBet { get; private set; }

        public GameEngine()
        {
            CreateDeck();
            RefreshHands();
            PlayerBet = new Bet(1000);
        }

        public GameEngine(Deck deck)
        {
            Deck = deck;
            RefreshHands();
            PlayerBet = new Bet(1000);
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
            PlayerHasStood = false;
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

            GameState? checkForBlackJack = CheckForBlackjack();

            if (checkForBlackJack != null) PlayerBet.CalculateWinnings(checkForBlackJack);

            return checkForBlackJack ?? GameState.InProgress;
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

            if (PlayerHand.GetValue() > 21)
            {
                PlayerBet.CalculateWinnings(GameState.PlayerBusted);
                return GameState.PlayerBusted;
            }

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

            if (DealerHand.GetValue() > 21)
            {
                PlayerBet.CalculateWinnings(GameState.DealerBusted);
                return GameState.DealerBusted;
            }

            PlayerHasStood = true;

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

            if (playerValue == dealerValue)
            {
                PlayerBet.CalculateWinnings(GameState.Push);
                return GameState.Push;
            }

            var state = playerValue > dealerValue ? GameState.PlayerWin : GameState.DealerWin;
            PlayerBet.CalculateWinnings(state);
            return state;


        }

        public bool IsRoundFinished()
        {
            var outcome = PeekState();
            PlayerBet.CalculateWinnings(outcome);
            return outcome != GameState.InProgress;
        }

        public GameState PeekState()
        {
            int playerValue = PlayerHand.GetValue();
            int dealerValue = DealerHand.GetValue();

            // Check for natural blackjack first
            var blackjackState = CheckForBlackjack();
            if (blackjackState.HasValue) return blackjackState.Value;

            // Then check for regular 21 (3+ cards)
            if (playerValue == 21) return GameState.PlayerWin;
            if (dealerValue == 21) return GameState.DealerWin;

            if (playerValue > 21) return GameState.PlayerBusted;
            if (dealerValue > 21) return GameState.DealerBusted;

            if (PlayerHasStood && dealerValue >= 17)
            {
                // Use already calculated values instead of recalculating
                if (playerValue == dealerValue) return GameState.Push;
                return playerValue > dealerValue ? GameState.PlayerWin : GameState.DealerWin;
            }

            return GameState.InProgress;
        }

        /// <summary>
        /// Checks for natural blackjack conditions (exactly 2 cards totaling 21)
        /// </summary>
        /// <returns>GameState if blackjack detected, null otherwise</returns>
        private GameState? CheckForBlackjack()
        {
            bool playerBlackjack = PlayerHand.IsBlackjack();
            bool dealerBlackjack = DealerHand.IsBlackjack();

            if (playerBlackjack && dealerBlackjack) return GameState.Push;
            if (playerBlackjack) return GameState.PlayerBlackjack;
            if (dealerBlackjack) return GameState.DealerBlackjack;

            return null;
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

        public void RestorePlayerHasStood(bool hasStood)
        {
            PlayerHasStood = hasStood;
        }

        public void LoadHands(Hand playerHand, Hand dealerHand, bool playerHasStood, Bet bet)
        {
            var pCards = playerHand.Cards ?? new List<Card>();
            var dCards = dealerHand.Cards ?? new List<Card>();

            var newPlayerHand = new Hand(pCards, isDealer: false);
            var newDealerHand = new Hand(dCards, isDealer: true);

            PlayerHand = newPlayerHand;
            DealerHand = newDealerHand;

            PlayerBet = bet;
            RestorePlayerHasStood(playerHasStood);
        }
    }
}
