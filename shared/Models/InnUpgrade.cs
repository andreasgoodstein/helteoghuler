using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerShared.Models;

public class InnUpgrade : IApplicable
{
    public Guid PlayerId { get; set; }
    public InnUpgradeName Upgrade { get; set; }

    public void ApplyToGameState(ref GameState gameState, Guid? _ = null)
    {
        var inn = gameState.GetPlayer(PlayerId).Inn;

        inn.Chest.Gold -= InnUpgrades.Cost[Upgrade];
        inn.PendingUpgrade = Upgrade;
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _ = null)
    {
        var inn = gameState.GetPlayer(PlayerId).Inn;

        inn.PendingUpgrade = null;
        inn.Chest.Gold += InnUpgrades.Cost[Upgrade];
    }
}
