namespace HelteOgHulerShared.Models;

public sealed class DodgeAction : HHAction
{
    public Action<Encounter, Random> TakeAction { get; set; }
}

public static partial class Actions
{
    public static readonly DodgeAction Dodge = new()
    {
        Name = ActionName.Dodge,
        Target = ActionTarget.Self,
        Outcome = "ACTOR prepares to dodge the coming attacks.",

        TakeAction = (Encounter encounter, Random random) =>
        {
            encounter.ActionLog.Add(Dodge.Outcome.Replace("ACTOR", encounter.CurrentlyActing.Name));

            // TODO: Figure out setting (and clearing) abilities dynamically
        }
    };
}