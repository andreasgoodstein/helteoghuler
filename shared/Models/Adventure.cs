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

    public Adventure(Hero[] party, int maxEncounters = 1)
    {
        Party = party;
        Status = Enum.GetName(typeof(EncounterStatus), EncounterStatus.Won);

        for (int n = 0; n < maxEncounters; n += 1)
        {
            Encounter encounter = new(null);
            encounter.ResolveEncounter(Party);

            EncounterList.Add(encounter);

            if (encounter.Status != EncounterStatus.Won)
            {
                Status = Enum.GetName(typeof(EncounterStatus), encounter.Status);
                RestUntil = DateTime.UtcNow.AddSeconds(REST_TIME_SEC * 2);
                return;
            }

            Gold += encounter.Reward;
        }

        RestUntil = DateTime.UtcNow.AddSeconds(REST_TIME_SEC);
    }

    public void ApplyToGameState(ref GameState gameState, Guid? id)
    {
        if (id == null)
        {
            return;
        }

        Guid playerId = (Guid)id;

        gameState.World.TotalAdventures += 1;

        gameState.PublicPlayerDict[playerId].TotalAdventures += 1;
        gameState.PublicPlayerDict[playerId].TotalGoldEarned += Gold;

        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold += Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = RestUntil;
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? id)
    {
        if (id == null)
        {
            return;
        }

        Guid playerId = (Guid)id;

        gameState.World.TotalAdventures -= 1;

        gameState.PublicPlayerDict[playerId].TotalAdventures -= 1;
        gameState.PublicPlayerDict[playerId].TotalGoldEarned -= Gold;

        gameState.PrivatePlayerDict[playerId].Inn.Chest.Gold -= Gold;
        gameState.PrivatePlayerDict[playerId].RestUntil = null;
    }
}
