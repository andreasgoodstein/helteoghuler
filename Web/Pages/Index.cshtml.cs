using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Web.Data;
using Web.Logic;

namespace Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class IndexModel : GameModel
    {
        public void OnGet()
        {
            ReadGameStateFromSession();
        }

        public IActionResult OnPost(IFormCollection data)
        {
            ReadGameStateFromSession();

            GameAction formAction = Enum.Parse<GameAction>(data["action"]);

            GameActionHandler.HandleGameAction(ref GameState, formAction);

            WriteGameStateToSession();

            return RedirectToPage();
        }
    }
}