using Web.Data;

namespace Web.Logic
{
    public static class GameActionHandler
    {
        public static void HandleGameAction(ref GameState state, GameAction action)
        {
            switch (action)
            {
                case GameAction.Drink:
                    {
                        DrinkHandler.HandleDrinkAction(state);
                        break;
                    }

                case GameAction.Attack:
                    {
                        CombatHandler.HandleAttackAction(state);
                        break;
                    }

                case GameAction.EnterBeerhalla:
                    {
                        BeerhallaHandler.HandleEntryInBeerhalla(state);
                        RestartGame(ref state);
                        break;
                    }

                case GameAction.GoToCave:
                    {
                        state.Location = GameLocation.Hulen;
                        break;
                    }

                case GameAction.GoToInn:
                    {
                        state.Location = GameLocation.Værtshuset;
                        break;
                    }

                case GameAction.Restart:
                    {
                        RestartGame(ref state);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private static void RestartGame(ref GameState state)
        {
            state = new GameState();
        }
    }
}