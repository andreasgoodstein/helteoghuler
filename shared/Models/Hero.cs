using HelteOgHulerShared.Interfaces;
using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Hero : IEncounterActor
{
    public ulong HP { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ulong Price { get; set; }
    public Dictionary<ActionName, HHAction> ActionList { get; set; } = Actions.DefaultActions;

    public void TakeAction(Encounter encounter)
    {
        ActionList[ActionName.Attack].TakeAction(encounter);

        // TODO: Implement ability selection
        // Random dice = new Random();

        // var roll = dice.Next(1, 100);

        // if (roll <= 75)
        // {
        //     ActionList[ActionName.Attack].TakeAction(encounter);
        // }
        // else
        // {
        //     ActionList[ActionName.Dodge].TakeAction(encounter);
        // }
    }
}
