using BOMBSQUAD.Models;
using Microsoft.AspNetCore.Mvc;

namespace BOMBSQUAD.Controllers
{
    public class HomeController : Controller
    {
        private static readonly BombSquad game = new BombSquad();

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reset()
        {
            game.Reset();
            return View("Game", game);
        }

        public IActionResult Click(string cell)
        {
            game.ClickCell(cell);
            return View("Game", game);
        }

        public IActionResult Flag(string cell)
        {
            game.FlagCell(cell);
            return View("Game", game);
        }
    }
}
