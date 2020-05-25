using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web
{
    public class GameModel : PageModel
    {
        private const string GAME_STATE_KEY = "@@helte_og_huler/game_state";

        public GameState GameState = new GameState();

        internal void ReadGameStateFromSession()
        {
            string gameStateString = HttpContext.Session.GetString(GAME_STATE_KEY) ?? "{}";

            GameState = JsonSerializer.Deserialize<GameState>(gameStateString);
        }

        internal void WriteGameStateToSession()
        {
            string gameStateString = JsonSerializer.Serialize(GameState);

            HttpContext.Session.SetString(GAME_STATE_KEY, gameStateString);
        }
    }
}