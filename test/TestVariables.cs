using HelteOgHulerShared.Models;

public static class TestVariables
{
    public static readonly DateTime DateTime = DateTime.Parse("2020-01-01T12:00");
    public static readonly Guid PlayerId = new("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeee000");
    public static readonly string HeroId = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeee001";


    public static GameState GetGameState() => new()
    {
        CurrentTime = DateTime,
        PrivatePlayerDict = new() {
            {
                PlayerId, new Player() {
                    Id = PlayerId,
                    Inn = new Inn() {
                        Chest = new() {
                            Gold = 0,
                        },
                        HeroRecruits = new() {
                            {
                                HeroId, new() {
                                    Id = new Guid(HeroId),
                                    Price = 200,
                                }
                            },
                        },
                    },
                }
            },
        },
    };
}