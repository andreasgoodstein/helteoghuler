#nullable enable

using HelteOgHulerShared.Interfaces;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HelteOgHulerShared.Models;

public class Encounter
{
    private const ulong MAX_ENCOUNTER_TURNS = 99;

    [IgnoreDataMember]
    public Hero[] Party { get; set; } = [];
    public Monster? Monster { get; set; }
    public ulong Reward { get; set; }
    [IgnoreDataMember]
    public Queue<IEncounterActor> InitiativeOrder { get; set; } = new Queue<IEncounterActor>();
    [IgnoreDataMember]
    public IEncounterActor? CurrentlyActing { get; set; }
    public EncounterStatus Status { get; set; } = EncounterStatus.Unresolved;
    public List<string> ActionLog { get; set; } = [];

    public void ResolveEncounter(Hero[] party, Random? random)
    {
        Debug.Assert(party.Length > 0, "IllegalState: Cannot adventure without a Party.");

        random ??= new Random();

        Party = party;
        Reward = (ulong)random.Next(1, 10);

        GenerateMonster(random);

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

            CurrentlyActing.TakeAction(this, random);

            InitiativeOrder.Enqueue(CurrentlyActing);

            encounterTimer -= 1;
        }
    }

    private void GenerateMonster(Random random)
    {
        Monster = new()
        {
            HP = 2,
            Type = random.NextDouble() < .5 ? MonsterType.Bat : MonsterType.Rat
        };
        Monster.Name = $"The {Enum.GetName(typeof(MonsterType), Monster.Type)}";
    }
}

public enum EncounterStatus
{
    Unresolved = 0,
    Won = 1,
    Lost = 2,
}
