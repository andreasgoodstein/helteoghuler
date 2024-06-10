using HelteOgHulerShared.Models;
using System;

namespace HelteOgHulerShared.Interfaces;

public interface IApplicable
{
    public void ApplyToGameState(ref GameState gameState, Nullable<Guid> playerId);

    public void RemoveFromGameState(ref GameState gameState, Nullable<Guid> playerId);
}
