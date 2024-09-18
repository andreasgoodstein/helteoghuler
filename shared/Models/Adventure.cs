using HelteOgHulerShared.Interfaces;

namespace HelteOgHulerShared.Models;

public class Adventure : IApplicable
{
    const int REST_TIME_SEC = 10;

    public ulong Gold { get; set; }
    public string Status { get; set; }
    public List<Encounter> EncounterList { get; set; } = [];
    public Hero[] Party { get; set; }
    public DateTime RestUntil { get; set; }

    public void ResolveAdventure(Hero[] party, int maxEncounters = 1)
    {
        // Setup
        Party = party;
        Status = Enum.GetName(typeof(EncounterStatus), EncounterStatus.Unresolved);

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
                Status = Enum.GetName(typeof(EncounterStatus), encounter.Status);
                RestUntil = DateTime.UtcNow.AddSeconds(REST_TIME_SEC * 2);
                return;
            }

            Gold += encounter.Reward;
        }

        // Finalize
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

        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold += Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = RestUntil;
        gameState.PrivatePlayerDict[playerId].LatestAdventure = this;
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

        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold -= Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = null;
        gameState.PrivatePlayerDict[playerId].LatestAdventure = null;
    }
}
