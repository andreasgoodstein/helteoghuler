using HelteOgHulerShared.Interfaces;

namespace HelteOgHulerShared.Models;

public class HHError : IApplicable
{
    public string Message { get; set; }

    public void ApplyToGameState(ref GameState gameState)
    {
        gameState.ErrorMessage = Message;
    }

    public void RemoveFromGameState(ref GameState gameState)
    {
        gameState.ErrorMessage = null;
    }
}
