namespace HelteOgHulerTest;

using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Moq;

public class InnUpgradeTest
{
    private readonly InnUpgrade innUpgrade = new()
    {
        PlayerId = TestVariables.PlayerId,
        Upgrade = InnUpgradeName.RenovateWorkshop,
    };

    [Fact]
    public void InnUpgradeAppliesCorrectlyToGameState()
    {
        var gameState = TestVariables.GetGameState();
        var inn = gameState.GetPlayer(TestVariables.PlayerId).Inn;
        inn.Chest.Gold = 35;

        inn.AvailableUpgrades = [innUpgrade.Upgrade];

        Assert.Null(inn.PendingUpgrade);

        innUpgrade.ApplyToGameState(ref gameState);

        Assert.Equal(innUpgrade.Upgrade, inn.PendingUpgrade);
        Assert.Equal((ulong)10, inn.Chest.Gold);
    }

    [Fact]
    public void InnUpgradeRemovesCorrectlyFromGameState()
    {
        var gameState = TestVariables.GetGameState();
        var inn = gameState.GetPlayer(TestVariables.PlayerId).Inn;
        inn.Chest.Gold = 35;

        innUpgrade.ApplyToGameState(ref gameState);
        innUpgrade.RemoveFromGameState(ref gameState);

        Assert.Null(inn.PendingUpgrade);
        Assert.Equal((ulong)35, inn.Chest.Gold);
    }
}