using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using Web.Data;
using Web.Logic;
using Web.Storage;

namespace Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class IndexModel : GameModel
    {
        public void OnGet()
        {
            LoadGameState();
        }

        public IActionResult OnPost(IFormCollection data)
        {
            LoadGameState();

            GameAction formAction = Enum.Parse<GameAction>(data["action"]);

            string heroName = data["hero-name"];

            GameState.HeroName = GetSanitizedHeroName(heroName);

            GameActionHandler.HandleGameAction(ref GameState, formAction);

            WriteGameStateToSession();

            return RedirectToPage();
        }

        private void LoadGameState()
        {
            ReadGameStateFromSession();

            GameState.Highscore = HighscoreStore.GetHighscore();

            GameState.Highscore.Sort(SortByBeerCountAndTimeOfDeath);
        }

        private string GetSanitizedHeroName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "";
            }

            return Regex.Replace(name, @"[^\w\s\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
        }

        private int SortByBeerCountAndTimeOfDeath(Hero a, Hero b)
        {
            if (a.BeerCount == b.BeerCount)
            {
                return a.TimeOfDeath > b.TimeOfDeath ? 1 : -1;
            }

            return a.BeerCount > b.BeerCount ? -1 : 1;
        }
    }
}