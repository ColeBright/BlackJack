using BlackJack.Game;
using BlackJack.Game.GameModels;
using BlackJack.Game.UiSession;
using BlackJack.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BlackJack.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;

        private const string SessionKey = "BlackJack.UiState";

        private UiState? LoadState() => HttpContext.Session.GetUiState(SessionKey);

        public GameController(ILogger<GameController> logger) 
        {
            _logger = logger;
        }

        private void SaveState(UiState state) => HttpContext.Session.SetUiState(SessionKey, state);

        private GameEngine RehydrateEngine(UiState state)
        {
            var deckCards = state.Deck.Select(CardSerialization.FromKey).ToList();
            var deck = new Deck(deckCards);

            var engine = new GameEngine(deck);

            engine.LoadHands(state.PlayerCards, state.DealerCards);

            return engine;
        }

        private UiState EngineToState(GameEngine engine)
        {
            var deckKeys = engine.Deck.Cards.Select(CardSerialization.ToKey).ToList();
            var playerKeys = engine.PlayerHand.Cards.Select(CardSerialization.ToKey).ToList();
            var dealerKeys = engine.DealerHand.Cards.Select(CardSerialization.ToKey).ToList();

            return new UiState(deckKeys, playerKeys, dealerKeys);

        }


        public IActionResult Game()
        {
            var state = LoadState();
            if (state == null) return RedirectToAction("Start");

            var engine = RehydrateEngine(state);

            return View("Game", engine);
        }

        public IActionResult Start()
        {
            var engine = new GameEngine();
            var stateBefore = EngineToState(engine);
            engine.StartRound();

            var state = EngineToState(engine);
            SaveState(state);

            //catch blackjack or push

            return View("Game", engine);
        }

        public IActionResult Hit()
        {
            var state = LoadState();
            if (state == null) return RedirectToAction("Start");

            var engine = RehydrateEngine(state);
            var outcome = engine.PlayerHit();

            SaveState(EngineToState(engine));

            if (outcome == GameState.PlayerBusted)
                return RedirectToAction("Result");


            return View("Game", engine);
        }

        public IActionResult Stand()
        {
            var state = LoadState();
            if (state == null) return RedirectToAction("Start");

            var engine = RehydrateEngine(state);
            var final = engine.PlayerStand();

            SaveState(EngineToState(engine));

            return RedirectToAction("Result");
        }

        public IActionResult Result()
        {
            var state = LoadState();
            if (state == null) return RedirectToAction("Start");

            var engine = RehydrateEngine(state);
            return View("Result", engine);
        }
    }
}
