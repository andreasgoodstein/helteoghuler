namespace HelteOgHulerSharedTest;

using HelteOgHulerShared.Models;
using HelteOgHulerShared.Interfaces;

public class EncounterTest
{
    [Fact]
    public void Constructor()
    {
        Encounter encounter = new();

        Assert.Equal(EncounterStatus.Unresolved, encounter.Status);
    }
}