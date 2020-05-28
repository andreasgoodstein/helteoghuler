using System;
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

            HighscoreStore.AddHeroToHighscore(newHero);
        }
    }
}