namespace HelteOgHulerTest;

using HelteOgHulerShared.Models;
using Moq;

public class EncounterTest
{
    private readonly Hero[] TestParty = [
        new() { ActionList = Actions.DefaultActions, HP = 2, Name = "TestHero" }
    ];

    [Fact]
    public void Constructor()
    {
        Encounter encounter = new(null);

        Assert.Equal(EncounterStatus.Unresolved, encounter.Status);
        Assert.Equal((ulong)2, encounter.Monster.HP);

        Assert.Empty(encounter.ActionLog);
        Assert.Empty(encounter.InitiativeOrder);
        Assert.Empty(encounter.Party);

        Assert.InRange<ulong>(encounter.Reward, 1, 10);
    }

    [Fact]
    public void ResolvesEncounterByExhaustion()
    {
        var RandomMock = new Mock<Random>();
        RandomMock.Setup(random => random.NextDouble()).Returns(.01);

        Encounter encounter = new();
        encounter.ResolveEncounter(TestParty, RandomMock.Object);

        Assert.Equal(EncounterStatus.Lost, encounter.Status);
        Assert.Equal("Your Party became exhausted and returned to the Inn.", encounter.ActionLog.Last());
    }

    [Fact]
    public void ResolvesEncounterByDefeat()
    {
        var RandomMock = new Mock<Random>();
        RandomMock.SetupSequence(random => random.NextDouble())
            .Returns(.05) // Monster Type = Bat
            .Returns(.99) // Monster Attack
            .Returns(.01) // Hero Attack
            .Returns(.99);

        Encounter encounter = new();
        encounter.ResolveEncounter(TestParty, RandomMock.Object);

        Assert.Equal(EncounterStatus.Lost, encounter.Status);
        Assert.Equal("TestHero is knocked unconscious.", encounter.ActionLog.Last());
    }

    [Fact]
    public void ResolvesEncounterByVictory()
    {
        var RandomMock = new Mock<Random>();
        // RandomMock.Setup(random => random.Next()).Returns(0);
        RandomMock.SetupSequence(random => random.NextDouble())
            .Returns(.05) // Monster Type = Bat
            .Returns(.49) // Monster Attack
            .Returns(.51) // Hero Attack
            .Returns(.49)
            .Returns(.51);

        Encounter encounter = new();
        encounter.ResolveEncounter(TestParty, RandomMock.Object);

        Assert.Equal(EncounterStatus.Won, encounter.Status);
        Assert.Equal("The Bat is knocked unconscious.", encounter.ActionLog.Last());
    }
}
