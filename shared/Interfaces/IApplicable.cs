using HelteOgHulerShared.Models;

namespace HelteOgHulerShared.Interfaces;

public interface IApplicable
{
    public void ApplyToGameState(ref GameState gameState);

    public void RemoveFromGameState(ref GameState gameState);
}
