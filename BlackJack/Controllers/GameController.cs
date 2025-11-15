using BlackJack.Game;
using Microsoft.AspNetCore.Mvc;

namespace BlackJack.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;

        private GameEngine _engine = new GameEngine();
        public GameController(ILogger<GameController> logger) 
        {
            _logger = logger;
             //_engine = engine;
        }


        public IActionResult Game()
        {
            return View("Game", _engine);
        }

        public IActionResult Start()
        {
            var result = _engine.StartRound();

            return View("Game", _engine);
        }

        public IActionResult Hit()
        {
            var result = _engine.PlayerHit();

            return View("Game", _engine);
        }

        public IActionResult Stand()
        {
            var result = _engine.PlayerStand();

            return RedirectToAction("Result");
        }

        public IActionResult Result()
        {
            return View("Result", _engine);
        }
    }
}
