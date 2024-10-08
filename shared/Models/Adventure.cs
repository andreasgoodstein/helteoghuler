using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Utilities;
using System.Diagnostics;

namespace HelteOgHulerShared.Models;

public class Adventure : IApplicable
{
    const int REST_TIME_SEC = 10;

    public ulong Gold { get; set; }
    public string Status { get; set; } = Enum.GetName(typeof(EncounterStatus), EncounterStatus.Unresolved);
    public List<Encounter> EncounterList { get; set; } = [];
    public Hero[] Party { get; set; }
    public DateTime RestUntil { get; set; }

    public void ResolveAdventure(Hero[] party, int maxEncounters = 1)
    {
        // Assert
        Debug.Assert(party.Length > 0, "IllegalState: Cannot start Adventure without a Party.");

        // Setup
        Party = party;

        foreach (Hero hero in Party)
        {
            hero.HP = hero.MaxHP;
        }

        // Resolve
        for (int n = 0; n < maxEncounters; n += 1)
        {
            Encounter encounter = new();
            encounter.ResolveEncounter(Party, null);

            EncounterList.Add(encounter);

            if (encounter.Status == EncounterStatus.Lost)
            {
                Status = Enum.GetName(typeof(EncounterStatus), EncounterStatus.Lost);
                RestUntil = DateTime.UtcNow.AddSeconds(REST_TIME_SEC * 2);
                return;
            }

            Gold += encounter.Reward;
        }

        // Finalize
        Status = Enum.GetName(typeof(EncounterStatus), EncounterStatus.Won);
        RestUntil = DateTime.UtcNow.AddSeconds(REST_TIME_SEC);
    }

    public void ApplyToGameState(ref GameState gameState, Guid? id)
    {
        gameState.World.TotalAdventures += 1;

        if (id == null)
        {
            return;
        }

        Guid playerId = (Guid)id;

        gameState.PublicPlayerDict[playerId].TotalAdventures += 1;
        gameState.PublicPlayerDict[playerId].TotalGoldEarned += Gold;

        var player = gameState.GetPlayer(playerId);

        player.Inn.Chest.Gold += Gold;
        player.RestUntil = RestUntil;
        player.LatestAdventure = this;
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? id)
    {
        gameState.World.TotalAdventures -= 1;

        if (id == null)
        {
            return;
        }

        Guid playerId = (Guid)id;

        gameState.PublicPlayerDict[playerId].TotalAdventures -= 1;
        gameState.PublicPlayerDict[playerId].TotalGoldEarned -= Gold;

        var player = gameState.GetPlayer(playerId);

        player.Inn.Chest.Gold -= Gold;
        player.RestUntil = null;
        player.LatestAdventure = null;
    }
}
