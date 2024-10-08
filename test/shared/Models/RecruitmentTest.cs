namespace HelteOgHulerTest;

using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Moq;

public class RecruitmentTest
{
    private readonly Recruitment recruitment = new()
    {
        HeroId = new Guid(TestVariables.HeroId),
        PlayerId = TestVariables.PlayerId
    };

    [Fact]
    public void RecruitmentAppliesCorrectlyToGameState()
    {
        var gameState = TestVariables.GetGameState();
        var inn = gameState.GetPlayer(TestVariables.PlayerId).Inn;
        inn.Chest.Gold = 210;

        Assert.Null(inn.HeroRoster);
        Assert.Single(inn.HeroRecruits);

        recruitment.ApplyToGameState(ref gameState);

        Assert.Empty(inn.HeroRecruits);
        Assert.Single(inn.HeroRoster);
        Assert.Equal((ulong)10, inn.Chest.Gold);
    }

    [Fact]
    public void RecruitmentRemovesCorrectlyFromGameState()
    {
        var gameState = TestVariables.GetGameState();
        var inn = gameState.GetPlayer(TestVariables.PlayerId).Inn;
        inn.Chest.Gold = 210;

        recruitment.ApplyToGameState(ref gameState);
        recruitment.RemoveFromGameState(ref gameState);

        Assert.Single(inn.HeroRecruits);
        Assert.Empty(inn.HeroRoster);
        Assert.Equal((ulong)210, inn.Chest.Gold);
    }
}