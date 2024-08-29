using HelteOgHulerShared.Interfaces;
using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Monster : IEncounterActor
{
    public ulong HP { get; set; }
    public string Name { get; set; }
    public Dictionary<ActionName, HHAction> ActionList { get; set; } = Actions.DefaultActions;
    public MonsterAbility[] AbilityList { get; set; }
    public MonsterType Type { get; set; }

    public Monster(Random random)
    {
        HP = 2;
        Type = random.NextDouble() < .5 ? MonsterType.Bat : MonsterType.Rat;
        Name = $"The {Enum.GetName(typeof(MonsterType), Type)}";
    }

    public void TakeAction(Encounter encounter, Random random)
    {
        ActionList[ActionName.Attack].TakeAction(encounter, random);

        // TODO: Implement ability selection
        // Random dice = new Random();

        // var roll = dice.Next(1, 100);

        // switch (Type)
        // {
        //     case MonsterType.Bat:
        //         if (roll <= 25)
        //         {
        //             ActionList[ActionName.Attack].TakeAction(encounter);
        //         }
        //         else
        //         {
        //             ActionList[ActionName.Dodge].TakeAction(encounter);
        //         }
        //         break;

        //     case MonsterType.Rat:
        //         if (roll <= 75)
        //         {
        //             ActionList[ActionName.Attack].TakeAction(encounter);
        //         }
        //         else
        //         {
        //             ActionList[ActionName.Dodge].TakeAction(encounter);
        //         }
        //         break;
        // }
    }
}

public enum MonsterAbility
{
    Dodging = 0,
    Flying = 1
}

public enum MonsterType
{
    Rat = 0,
    Bat = 1
}
