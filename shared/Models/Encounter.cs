#nullable enable

using HelteOgHulerShared.Interfaces;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HelteOgHulerShared.Models;

public class Encounter
{
    private const ulong MAX_ENCOUNTER_TURNS = 99;
    private Random Random;

    [IgnoreDataMember]
    public Hero[] Party { get; set; } = [];
    public Monster Monster { get; set; }
    public ulong Reward { get; set; }
    [IgnoreDataMember]
    public Queue<IEncounterActor> InitiativeOrder { get; set; } = new Queue<IEncounterActor>();
    [IgnoreDataMember]
    public IEncounterActor? CurrentlyActing { get; set; }
    public EncounterStatus Status { get; set; } = EncounterStatus.Unresolved;
    public List<string> ActionLog { get; set; } = [];

    public Encounter(Random? random)
    {
        Random = random ?? new Random();

        Monster = new(Random);
        Reward = (ulong)Random.Next(1, 10);
    }

    public void ResolveEncounter(Hero[] party)
    {
        Debug.Assert(party.Length > 0);

        Party = party;

        InitiativeOrder.Enqueue(Monster);

        foreach (Hero hero in Party)
        {
            InitiativeOrder.Enqueue(hero);
        }

        var encounterTimer = MAX_ENCOUNTER_TURNS;

        while (Status == EncounterStatus.Unresolved)
        {
            if (encounterTimer <= 0)
            {
                ActionLog.Add("Your Party became exhausted and returned to the Inn.");
                Status = EncounterStatus.Lost;
                break;
            }

            CurrentlyActing = InitiativeOrder.Dequeue();

            CurrentlyActing.TakeAction(this, Random);

            InitiativeOrder.Enqueue(CurrentlyActing);

            encounterTimer -= 1;
        }
    }
}

public enum EncounterStatus
{
    Unresolved = 0,
    Won = 1,
    Lost = 2,
}
