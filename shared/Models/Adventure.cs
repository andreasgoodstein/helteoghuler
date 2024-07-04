using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class Adventure : IApplicable
{
    public ulong Gold { get; set; }

    public string Status { get; set; }

    public DateTime RestUntil { get; set; }

    public void ApplyToGameState(ref GameState gameState, Guid? id)
    {
        if (id == null)
        {
            return;
        }

        Guid playerId = (Guid)id;

        gameState.World.TotalAdventures += 1;

        gameState.PublicPlayerDict[playerId].TotalGoldEarned += Gold;

        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold += Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = RestUntil;
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? id)
    {
        if (id == null)
        {
            return;
        }

        Guid playerId = (Guid)id;

        gameState.World.TotalAdventures -= 1;

        gameState.PublicPlayerDict[playerId].TotalGoldEarned -= Gold;

        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold -= Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = null;
    }
}
