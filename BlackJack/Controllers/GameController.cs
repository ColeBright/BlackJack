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

        private GameEngine? LoadEngine()
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
            // Ensure we always have an engine in session so betting UI can work
            var state = LoadEngine();
            if (state == null)
            {
                state = new GameEngine();
                SaveState(state);
            }

            return View("Game", state);
        }

        [HttpPost]
        public IActionResult PlaceBet(decimal amount)
        {
            // Work against the existing engine so balance persists between rounds
            var engine = LoadEngine() ?? new GameEngine();

            try
            {
                engine.PlaceBet(amount);
                SaveState(engine);
            }
            catch (Exception ex) 
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Game");
        }

        [HttpPost]
        public IActionResult Start()
        {
            try
            {
                var engine = LoadEngine() ?? new GameEngine();

                if (engine.PlayerBet == null || engine.PlayerBet.CurrentBet <= 0 || !engine.PlayerBet.IsActive)
                {
                    TempData["Error"] = "Place a bet first!";
                    SaveState(engine);
                    return RedirectToAction("Game");
                }

                engine.StartRound();

                SaveState(engine);

                return RedirectToAction("Game");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting new round");
                TempData["Error"] = ex.Message;
                return RedirectToAction("Game");
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
