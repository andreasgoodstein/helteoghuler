using HelteOgHulerShared.Interfaces;
using System;
using System.Collections.Generic;

namespace HelteOgHulerShared.Models;

public class Encounter
{
    private const ulong MAX_ENCOUNTER_TURNS = 99;

    public Hero[] Party { get; set; }
    public Monster Monster { get; set; }
    public ulong Reward { get; set; }
    public Queue<IEncounterActor> InitiativeOrder { get; set; } = new Queue<IEncounterActor>();
    public IEncounterActor CurrentlyActing { get; set; }
    public EncounterStatus Status { get; set; } = EncounterStatus.Unresolved;
    public List<string> ActionLog { get; set; } = new List<string>();

    public void ResolveEncounter()
    {
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
                ActionLog.Add("Your Party became exhausted and returned to your Inn.");
                Status = EncounterStatus.Lost;
                return;
            }

            CurrentlyActing = InitiativeOrder.Dequeue();

            CurrentlyActing.TakeAction(this);

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