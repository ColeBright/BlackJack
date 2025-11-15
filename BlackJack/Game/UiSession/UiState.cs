namespace BlackJack.Game.UiSession
{
    public record UiState(
        List<string> Deck,
        List<string> PlayerCards,
        List<string> DealerCards,
        bool PlayerHasStood = false
        );
}
