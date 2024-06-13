using HelteOgHulerShared.Models;
using System;

namespace HelteOgHulerShared.Interfaces;

public interface IApplicable
{
    public void ApplyToGameState(ref GameState gameState, Guid? playerId);

    public void RemoveFromGameState(ref GameState gameState, Guid? playerId);
}
