using System.Security.Cryptography;

namespace Web.Logic
{
    public static class CombatHandler
    {
        private const int MAX_DAMAGE = 2;

        private const int MAX_GOLD = 5;

        public static void HandleAttackAction(GameState state)
        {
            int adjustedMaxDamage = MAX_DAMAGE + state.MaxHealth / 2;

            int battleDamage = RandomNumberGenerator.GetInt32(adjustedMaxDamage);

            state.Health -= battleDamage;

            state.UpdateLog($"Monsteret banker dig for {battleDamage} skade. Av!");

            bool heroDiesInBattle = state.Health < 1;
            if (heroDiesInBattle)
            {
                state.Location = Data.GameLocation.Graven;

                state.UpdateLog("Det er for meget for en helt som dig. Du har drukket din sidste øl!");

                return;
            }

            int monsterGold = RandomNumberGenerator.GetInt32(MAX_GOLD);

            state.Gold += monsterGold;

            state.UpdateLog($"Du banker monsteret tilbage. Pow! Det taber {monsterGold} guld, som du putter i lommen.");
        }
    }
}