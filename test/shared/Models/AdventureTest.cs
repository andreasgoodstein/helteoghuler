namespace HelteOgHulerTest;

using System.Linq;
using HelteOgHulerShared.Models;

public class AdventureTest
{
    private readonly Hero[] TestParty = [
        new() { HP = 2, Name = "TestHero" }
    ];

    [Fact]
    public void Constructor()
    {
        Adventure adventure = new();
        adventure.ResolveAdventure(TestParty);

        Assert.Single(adventure.EncounterList);

        Assert.NotEqual(Enum.GetName(typeof(EncounterStatus), EncounterStatus.Unresolved), adventure.Status);

        if (Enum.GetName(typeof(EncounterStatus), EncounterStatus.Won) == adventure.Status)
        {
            var sumOfEncounterRewards = (ulong)adventure.EncounterList.Sum(encounter => (long)encounter.Reward);

            Assert.Equal(sumOfEncounterRewards, adventure.Gold);
        }

        if (Enum.GetName(typeof(EncounterStatus), EncounterStatus.Lost) == adventure.Status)
        {
            Assert.Equal((ulong)0, adventure.Gold);
        }
    }
}
