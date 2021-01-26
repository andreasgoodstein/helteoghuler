using System.Linq;
using System.Security.Cryptography;

namespace Web.Logic
{
    public static class CombatHandler
    {
        private const int MAX_DAMAGE = 2;

        private const int MAX_GOLD = 5;

        public static void HandleAttackAction(GameState state)
        {
            HandleDamage(state);

            bool heroDiesInBattle = state.Health < 1;
            if (heroDiesInBattle)
            {
                HandleHeroDeath(state);
                return;
            }

            HandleGold(state);
        }

        private static void HandleDamage(GameState state)
        {
            int adjustedMaxDamage = MAX_DAMAGE + state.MaxHealth / 2;

            int battleDamage = RandomNumberGenerator.GetInt32(adjustedMaxDamage);

            if (battleDamage < 1)
            {
                state.UpdateLog($"Monsteret slår ud efter dig, men du er for hurtig. Ha!");
                return;
            }

            state.Health -= battleDamage;

            state.UpdateLog($"Monsteret banker dig for {battleDamage} skade. Av!");
        }

        private static void HandleHeroDeath(GameState state)
        {
            state.UpdateLog("Det er for meget for en helt som dig. Du har drukket din sidste øl!");

            int minHighscore = state.Highscore.Count > 0
                ? state.Highscore.Min(hero => hero.BeerCount)
                : 0;

            bool heroReplacesOldHero = state.BeerCount > minHighscore;
            bool beerhallaNeedsHeroes = state.Highscore.Count < ConfigVariables.MAX_HIGHSCORE_SPOTS;
            bool heroHasBeer = state.BeerCount > 0;
            bool addHeroToHighscore = heroReplacesOldHero || (beerhallaNeedsHeroes && heroHasBeer);
            if (addHeroToHighscore)
            {
                state.Location = Data.GameLocation.Ølhalla;

                state.UpdateLog("Men vent! Et lys åbner sig i himmelen over dig, og duften af fadøl finder din næse...");
                return;
            }

            state.Location = Data.GameLocation.Graven;
        }

        private static void HandleGold(GameState state)
        {
            int monsterGold = RandomNumberGenerator.GetInt32(MAX_GOLD);

            state.Gold += monsterGold;

            state.UpdateLog($"Du banker monsteret tilbage. Pow! Det taber {monsterGold} guld, som du putter i lommen.");
        }
    }
}