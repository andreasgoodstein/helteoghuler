using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class Adventure : IApplicable
{
    public Nullable<Guid> Id { get; set; }

    public ulong Gold { get; set; }

    public string Status { get; set; }

    public DateTime RestUntil { get; set; }

    public void ApplyToGameState(ref GameState gameState)
    {
        gameState.World.TotalAdventures += 1;
        gameState.Player.Inn.Chest.Gold += Gold;
        gameState.Player.RestUntil = RestUntil;
    }

    public void RemoveFromGameState(ref GameState gameState)
    {
        gameState.World.TotalAdventures -= 1;
        gameState.Player.Inn.Chest.Gold -= Gold;
        gameState.Player.RestUntil = null;
    }
}
