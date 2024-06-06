using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class Adventure : IApplicable
{
    public Nullable<Guid> Id { get; set; }

    public ulong Gold { get; set; }

    public void ApplyToGameState(ref GameState gameState)
    {
        gameState.World.TotalAdventures += 1;
        gameState.Player.Inn.Chest.Gold += Gold;
    }

    public void RemoveFromGameState(ref GameState gameState)
    {
        if (gameState?.World?.TotalAdventures > 0 && gameState?.Player?.Inn?.Chest?.Gold >= Gold)
        {
            gameState.World.TotalAdventures -= 1;
            gameState.Player.Inn.Chest.Gold -= Gold;
        }
    }
}
