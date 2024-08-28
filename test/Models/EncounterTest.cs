namespace HelteOgHulerTest;

using HelteOgHulerShared.Models;

public class EncounterTest
{
    [Fact]
    public void Constructor()
    {
        Encounter encounter = new();

        Assert.Equal(EncounterStatus.Unresolved, encounter.Status);
    }
}