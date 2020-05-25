using System;
using System.Security.Cryptography;

namespace Web.Logic
{
    public static class DrinkHandler
    {
        private const int BEER_PRICE = 2;

        public static void HandleDrinkAction(GameState state)
        {
            bool heroCanBuyBeer = state.Gold >= BEER_PRICE;
            if (!heroCanBuyBeer)
            {
                state.UpdateLog("Du prøver at købe en øl, men du har ikke nok guld på lommen. Øv!");
                return;
            }

            state.Gold -= BEER_PRICE;
            state.BeerCount += 1;

            bool anotherFiveBeers = state.BeerCount % 5 == 0;
            if (anotherFiveBeers)
            {
                state.MaxHealth += 1;

                state.UpdateLog("Du har drukket så mange øl at du bliver en endnu mægtigere helt.");
            }

            bool beerHealsHero = RandomNumberGenerator.GetInt32(4) > 0;
            if (!beerHealsHero)
            {
                state.UpdateLog("Du drikker en øl. Den smager godt, men du er stadig tørstig.");
                return;
            }

            state.Health = Math.Min(state.MaxHealth, state.Health + 1);

            state.UpdateLog("Du bunder en øl, og får det lidt bedre.");
        }
    }
}