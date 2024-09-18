using HelteOgHulerShared.Interfaces;

namespace HelteOgHulerShared.Models;

public static partial class Actions
{
    private const int MAX_CHANCE = 100;
    private const int MIN_CHANCE = 1;

    public static readonly Dictionary<ActionName, HHAction> DefaultActions = [];

    private static IEncounterActor GetTarget(Encounter encounter, ActionTarget targetType, Random random)
    {
        return targetType switch
        {
            ActionTarget.Ally => (encounter.CurrentlyActing is Hero) ?
                                encounter.Party.Length > 1 ?
                                    encounter.Party.Where(acting => acting.Id != encounter.CurrentlyActing.Id).ToArray()[random.Next(0, encounter.Party.Length - 1)] :
                                    encounter.CurrentlyActing :
                                encounter.Monster,

            ActionTarget.Enemy => (encounter.CurrentlyActing is Hero) ?
                                encounter.Monster :
                                encounter.Party[random.Next(0, encounter.Party.Length)],

            // ActionTarget.Self
            _ => encounter.CurrentlyActing,
        };
    }
}

public enum ActionName
{
    Attack = 0,
    Dodge = 1,
}

public enum ActionTarget
{
    Self = 0,
    Ally = 1,
    Enemy = 2
}
