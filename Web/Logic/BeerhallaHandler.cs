using System;
using System.Linq;
using Web.Data;
using Web.Storage;

namespace Web.Logic
{
    public static class BeerhallaHandler
    {
        public static void HandleEntryInBeerhalla(GameState state)
        {
            Hero newHero = new Hero();
            newHero.TimeOfDeath = DateTime.UtcNow;
            newHero.BeerCount = state.BeerCount;
            newHero.Name = state.HeroName;

            if (state.Highscore.Count >= ConfigVariables.MAX_HIGHSCORE_SPOTS)
            {
                RemoveLeastWorthyHero(state);
            }

            HighscoreStore.AddHeroToHighscore(newHero);
        }

        private static void RemoveLeastWorthyHero(GameState state)
        {
            Hero oldHero = state.Highscore.Last();
            HighscoreStore.RemoveHeroFromHighscore(oldHero);
        }
    }
}