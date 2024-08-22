using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class Monster : ITurnAction
{
    public ulong HP { get; set; }
    public HHAction[] ActionList { get; set; } = Actions.DefaultActions;
    public MonsterAbility[] AbilityList { get; set; }
    public MonsterType Type { get; set; }

    public void TakeAction() { }
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
