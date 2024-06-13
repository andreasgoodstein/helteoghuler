using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class HHError : IApplicable
{
    public string Message { get; set; }

    public void ApplyToGameState(ref GameState gameState, Guid? playerId)
    {
        gameState.ErrorMessage = Message;
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? playerId)
    {
        gameState.ErrorMessage = null;
    }
}
