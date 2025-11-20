using BlackJack.DataTransfer.GameDtos;
using BlackJack.Domain.GameModels;
using BlackJack.Helpers;
using BlackJack.Web.DataTransfer.GameDtos;

namespace BlackJack.Game
{
    public static class GameEngineMapper
    {
        public static GameStateDto ToDto(this GameEngine engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            return new GameStateDto
            {
                DeckKeys = engine.Deck.Cards.Select(CardSerialization.ToKey).ToList(),
                PlayerHand = new HandDto
                {
                    CardKeys = engine.PlayerHand.Cards.Select(CardSerialization.ToKey).ToList(),
                    IsDealer = false
                },
                DealerHand = new HandDto
                {
                    CardKeys = engine.DealerHand.Cards.Select(CardSerialization.ToKey).ToList(),
                    IsDealer = true
                },
                PlayerHasStood = engine.PlayerHasStood,
                Bet = new BetDto
                {
                    CurrentBet = engine.PlayerBet.CurrentBet,
                    PlayerBalance = engine.PlayerBet.PlayerBalance,
                    IsActive = engine.PlayerBet.IsActive,
                }
            };
        }

        public static GameEngine FromDto(this GameStateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var deck = new Deck(dto.DeckKeys.Select(CardSerialization.FromKey));
            var engine = new GameEngine(deck);

            var playerHand = new Hand(dto.PlayerHand.CardKeys.Select(CardSerialization.FromKey).ToList());
            var dealerHand = new Hand(dto.DealerHand.CardKeys.Select(CardSerialization.FromKey).ToList(), isDealer: true);
            var playerHasStood = dto.PlayerHasStood;
            var bet = new Bet(dto.Bet.CurrentBet, dto.Bet.PlayerBalance, dto.Bet.IsActive);

            engine.LoadHands(playerHand, dealerHand, playerHasStood, bet);

            return engine;
        }
    }
}
