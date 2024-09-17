using HelteOgHulerShared.Interfaces;

namespace HelteOgHulerShared.Models;

public enum AttackModifier
{
    ToHit = 0,
    ToCrit = 1
}

public sealed class AttackAction : HHAction
{
    public readonly Dictionary<AttackModifier, int> Probabilities = new() {
        { AttackModifier.ToCrit, 95 } ,
        { AttackModifier.ToHit, 50 },
    };

    public Action<Encounter, Dictionary<AttackModifier, int>, Random> TakeAction { get; set; }
}

public static partial class Actions
{
    public static readonly AttackAction Attack = new()
    {
        Name = ActionName.Attack,
        Target = ActionTarget.Enemy,
        Outcome = "ACTOR takes a swing at TARGET. (Success: SUCCESS | Crit: CRIT)",

        TakeAction = (Encounter encounter, Dictionary<AttackModifier, int> modifiers, Random random) =>
        {
            IEncounterActor target;
            if (encounter.CurrentlyActing is Hero)
            {
                target = encounter.Monster;
            }
            else
            {
                target = encounter.Party[random.Next(0, encounter.Party.Length)];
            }

            modifiers.TryGetValue(AttackModifier.ToCrit, out int toCritModifier);
            modifiers.TryGetValue(AttackModifier.ToHit, out int toHitModifier);

            var toCrit = Math.Min(Math.Max(Attack.Probabilities[AttackModifier.ToCrit] + toCritModifier, 1), 100);
            var toHit = Math.Min(Math.Max(Attack.Probabilities[AttackModifier.ToHit] + toHitModifier, 1), 100);

            encounter.ActionLog.Add(Attack.Outcome
                .Replace("ACTOR", encounter.CurrentlyActing.Name)
                .Replace("TARGET", target.Name)
                .Replace("SUCCESS", toHit.ToString())
                .Replace("CRIT", toCrit.ToString())
            );

            int roll = random.Next(1, 100);
            bool doesAttackCrit = roll >= toCrit;
            bool doesAttackHit = doesAttackCrit || roll >= toHit;

            if (!doesAttackHit)
            {
                encounter.ActionLog.Add($"They roll a {roll} and miss!");
                return;
            }

            if (doesAttackCrit)
            {
                encounter.ActionLog.Add($"They roll a {roll} and critically hit! Doing 2 damage.");
                target.HP -= 2;
            }
            else
            {
                encounter.ActionLog.Add($"They roll a {roll} and hit! Doing 1 damage.");
                target.HP -= 1;
            }

            if (target.HP <= 0)
            {
                // Encounter over
                encounter.ActionLog.Add($"{target.Name} is knocked unconscious.");

                if (target is Hero)
                {
                    encounter.Status = EncounterStatus.Lost;
                }
                else
                {
                    encounter.Status = EncounterStatus.Won;
                }
            }
        }
    };
}