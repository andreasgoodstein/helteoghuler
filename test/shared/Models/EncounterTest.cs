namespace HelteOgHulerTest;

using HelteOgHulerShared.Models;
using Moq;

public class EncounterTest
{
    private readonly Hero[] TestParty = [
        new() { ActionList = [.. Actions.DefaultActions.Keys], HP = 2, Name = "TestHero" }
    ];

    [Fact]
    public void Constructor()
    {
        Encounter encounter = new();

        Assert.Equal(EncounterStatus.Unresolved, encounter.Status);

        Assert.Empty(encounter.ActionLog);
        Assert.Empty(encounter.InitiativeOrder);
        Assert.Empty(encounter.Party);
    }

    [Fact]
    public void ResolvesEncounterByExhaustion()
    {
        var RandomMock = new Mock<Random>();
        RandomMock.Setup(random => random.Next(1, 100)).Returns(1);

        Encounter encounter = new();
        encounter.ResolveEncounter(TestParty, RandomMock.Object);

        Assert.Equal(EncounterStatus.Lost, encounter.Status);
        Assert.Equal("Your Party became exhausted and returned to the Inn.", encounter.ActionLog.Last());
    }

    [Fact]
    public void ResolvesEncounterByDefeat()
    {
        var RandomMock = new Mock<Random>();
        RandomMock.SetupSequence(random => random.Next(1, 100))
            .Returns(50) // Monster Type = Bat
            .Returns(94) // Monster Attack
            .Returns(1) // Hero Attack
            .Returns(94);

        Encounter encounter = new();
        encounter.ResolveEncounter(TestParty, RandomMock.Object);

        // Assert.Equal((ulong)5, encounter.Reward);
        Assert.Equal(MonsterType.Bat, encounter.Monster!.Type); Assert.Equal(EncounterStatus.Lost, encounter.Status);
        Assert.Equal("TestHero is knocked unconscious.", encounter.ActionLog.Last());
    }

    [Fact]
    public void ResolvesEncounterByVictory()
    {
        var RandomMock = new Mock<Random>();
        // RandomMock.Setup(random => random.Next()).Returns(0);
        RandomMock.SetupSequence(random => random.Next(1, 100))
            .Returns(51) // Monster Type = Rat
            .Returns(49) // Monster Attack
            .Returns(51) // Hero Attack
            .Returns(49)
            .Returns(51);

        Encounter encounter = new();
        encounter.ResolveEncounter(TestParty, RandomMock.Object);

        // Assert.Equal((ulong)5, encounter.Reward);
        Assert.Equal(MonsterType.Rat, encounter.Monster!.Type);
        Assert.Equal(EncounterStatus.Won, encounter.Status);
        Assert.Equal("The Rat is knocked unconscious.", encounter.ActionLog.Last());
    }
}
