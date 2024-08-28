namespace HelteOgHulerTest;

using HelteOgHulerShared.Models;

public class EncounterTest
{
    [Fact]
    public void Constructor()
    {
        Encounter encounter = new();

        Assert.Equal(EncounterStatus.Unresolved, encounter.Status);
        Assert.Equal((ulong)2, encounter.Monster.HP);

        Assert.Empty(encounter.ActionLog);
        Assert.Empty(encounter.InitiativeOrder);

        Assert.Null(encounter.Party);

        Assert.InRange<ulong>(encounter.Reward, 1, 10);
    }
}
