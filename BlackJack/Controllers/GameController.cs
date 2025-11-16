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

        private GameEngine LoadEngine()
        {
            var dto = HttpContext.Session.GetGameState(SessionKey);
            if (dto == null) return null;
            return GameEngine.FromDto(dto);
        }

        public GameController(ILogger<GameController> logger) 
        {
            _logger = logger;
        }

        private void SaveState(GameEngine engine) => HttpContext.Session.SetGameState(SessionKey, engine.ToDto());

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Game()
        {
            var state = LoadEngine();
            if (state == null) return RedirectToAction("Index");

            return View("Game", state);
        }

        [HttpPost]
        public IActionResult Start()
        {
            var engine = new GameEngine();
            engine.StartRound();

            SaveState(engine);

            return RedirectToAction("Game");
        }

        [HttpPost]
        public IActionResult Hit()
        {
            var state = LoadEngine();
            if (state == null) return RedirectToAction("Index");

            state.PlayerHit();

            SaveState(state);

            return RedirectToAction("Game");
        }

        [HttpPost]
        public IActionResult Stand()
        {
            var state = LoadEngine();
            if (state == null) return RedirectToAction("Index");

            state.PlayerStand();

            SaveState(state);

            return RedirectToAction("Game");
        }

        public IActionResult Result()
        {
            var state = LoadEngine();
            if (state == null) return RedirectToAction("Start");

            return View("Game", state);
        }
    }
}
