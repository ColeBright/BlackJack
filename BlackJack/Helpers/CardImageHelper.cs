using BlackJack.Domain.GameModels;

namespace BlackJack.Helpers
{
    public static class CardImageHelper
    {
        public static string GetCardImagePath(Card card)
        {
            if (card == null) return "~/images/cards/back.png";

            string suit = card.Suit.ToString().ToLowerInvariant();

            string rank;

            int numeric = (int)card.Rank;

            if (numeric >= 2 && numeric <= 10)
            {
                rank = numeric.ToString();
            }
            else
            {
                switch (card.Rank)
                {
                    case Rank.Ace:
                        rank = "ace";
                        break;
                    case Rank.Jack:
                        rank = "jack";
                        break;
                    case Rank.Queen:
                        rank = "queen";
                        break;
                    case Rank.King:
                        rank = "king";
                        break;
                    default:
                        rank = card.Rank.ToString().ToLowerInvariant();
                        break;
                }
            }

            var fileName = $"{rank}_of_{suit}.png";
            return $"~/images/cards/{fileName}";
        }
    }
}
