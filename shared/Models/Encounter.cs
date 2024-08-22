using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class Encounter
{
    public Monster Monster { get; set; }

    public Turn[] TurnList { get; set; }

    public void ResolveEncounter(Hero[] party)
    {

    }
}

public class Turn
{
    public HHAction[] ActionList { get; set; }
}

public class HHAction
{
    public string Name { get; set; }
    public string Outcome { get; set; }
    public ActionTarget Target { get; set; }
}

public enum ActionTarget
{
    Self = 0,
    Ally = 1,
    Enemy = 2
}

public static class Actions
{
    public static HHAction[] DefaultActions = {
        new HHAction { Name = "Attack", Target = ActionTarget.Enemy },
        new HHAction { Name = "Dodge", Target = ActionTarget.Self }
    };
}