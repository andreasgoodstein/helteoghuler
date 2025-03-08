using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerShared.Models;

public class CompleteObjective : IApplicable
{
    public Guid PlayerId { get; set; }
    public PlayerObjective Objective { get; set; }

    public void ApplyToGameState(ref GameState gameState, Guid? playerId)
    {
        var player = GameStateHelper.GetPlayer(gameState, PlayerId);

        (player.ObjectivesCompleted ??= []).Add(Objective, true);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? playerId)
    {
        var player = GameStateHelper.GetPlayer(gameState, PlayerId);
        player.ObjectivesCompleted.Remove(Objective);
    }
}
