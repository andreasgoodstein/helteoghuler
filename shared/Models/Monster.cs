using HelteOgHulerShared.Interfaces;

namespace HelteOgHulerShared.Models;

public class Monster : IEncounterActor
{
    public ulong HP { get; set; }
    public string Name { get; set; }
    public MonsterAbility[] AbilityList { get; set; }
    public MonsterType Type { get; set; }
    public List<ActionName> ActionList { get; set; } = [.. Actions.DefaultActions.Keys];

    public void TakeAction(Encounter encounter, Random random)
    {
        Actions.Attack.TakeAction(encounter, [], random);

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
