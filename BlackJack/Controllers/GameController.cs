using BlackJack.Game;
using Microsoft.AspNetCore.Mvc;

namespace BlackJack.Controllers
{
    public class GameController : Controller
    {
        private GameEngine engine;
        public GameController() 
        {
             engine = new GameEngine();
        }


        public IActionResult Game()
        {
            return View("Game", engine);
        }

        public IActionResult Start()
        {
            var result = engine.StartRound();

            return View("Game", engine);
        }

        public IActionResult Hit()
        {
            var result = engine.PlayerHit();

            return View("Game", engine);
        }

        public IActionResult Stand()
        {
            var result = engine.PlayerStand();

            return View("Game", engine);
        }
    }
}
