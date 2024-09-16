namespace HelteOgHulerShared.Models;

public static partial class Actions
{
    public static readonly Dictionary<ActionName, HHAction> DefaultActions = [];
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
