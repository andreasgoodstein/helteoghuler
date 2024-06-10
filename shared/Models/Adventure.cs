using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class Adventure : IApplicable
{
    public Nullable<Guid> Id { get; set; }

    public ulong Gold { get; set; }

    public string Status { get; set; }

    public DateTime RestUntil { get; set; }

    public void ApplyToGameState(ref GameState gameState, Nullable<Guid> id)
    {
        if (id == null)
        {
            return;
        }

        var playerId = (Guid)id;

        gameState.World.TotalAdventures += 1;
        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold += Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = RestUntil;
    }

    public void RemoveFromGameState(ref GameState gameState, Nullable<Guid> id)
    {
        if (id == null)
        {
            return;
        }

        var playerId = (Guid)id;

        gameState.World.TotalAdventures -= 1;
        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold -= Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = null;
    }
}
