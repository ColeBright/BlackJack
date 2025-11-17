using BlackJack.Domain.GameModels;
using BlackJack.Game;
using BlackJack.Game.UiSession;
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
            return dto.FromDto();
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
            try
            {
                var engine = new GameEngine();
                engine.StartRound();

                SaveState(engine);

                return RedirectToAction("Game");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting new round");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Hit()
        {
            try
            {
                var state = LoadEngine();
                if (state == null) return RedirectToAction("Index");

                state.PlayerHit();

                SaveState(state);

                return RedirectToAction("Game");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing hit");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Stand()
        {
            try
            {
                var state = LoadEngine();
                if (state == null) return RedirectToAction("Index");

                state.PlayerStand();

                SaveState(state);

                return RedirectToAction("Game");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stand");
                return RedirectToAction("Index");
            }
        }

        public IActionResult Result()
        {
            var state = LoadEngine();
            if (state == null) return RedirectToAction("Start");

            return View("Game", state);
        }
    }
}
