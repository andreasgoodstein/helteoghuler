using HelteOgHulerShared.Interfaces;

namespace HelteOgHulerShared.Models;

using AttackModifierDict = Dictionary<AttackModifier, int>;

public static partial class Actions
{
    private const int MIN_ATTACK_DAMAGE = 1;

    public static readonly AttackAction Attack = new()
    {
        Name = ActionName.Attack,
        Target = ActionTarget.Enemy,
        Outcome = "ACTOR takes a swing at TARGET. (Success: SUCCESS | Crit: CRIT)",

        TakeAction = (Encounter encounter, AttackModifierDict modifiers, Random random) =>
        {
            IEncounterActor target = GetTarget(encounter, Attack.Target, random);

            var appliedModifiers = ApplyAttackModifiers(modifiers);

            encounter.ActionLog.Add(Attack.Outcome
                .Replace("ACTOR", encounter.CurrentlyActing.Name)
                .Replace("TARGET", target.Name)
                .Replace("SUCCESS", appliedModifiers[AttackModifier.HitChance].ToString())
                .Replace("CRIT", appliedModifiers[AttackModifier.CritChance].ToString())
            );

            int roll = random.Next(MIN_CHANCE, MAX_CHANCE);
            bool doesAttackCrit = roll >= appliedModifiers[AttackModifier.CritChance];
            bool doesAttackHit = doesAttackCrit || roll >= appliedModifiers[AttackModifier.HitChance];

            if (!doesAttackHit)
            {
                encounter.ActionLog.Add($"They roll a {roll} and miss!");
                return;
            }

            if (doesAttackCrit)
            {
                var damage = appliedModifiers[AttackModifier.CritDamage];
                encounter.ActionLog.Add($"They roll a {roll} and critically hit! Doing {damage} damage.");
                target.HP -= damage;
            }
            else
            {
                var damage = appliedModifiers[AttackModifier.HitDamage];
                encounter.ActionLog.Add($"They roll a {roll} and hit. Doing {damage} damage.");
                target.HP -= damage;
            }

            // TODO: Consider moving encounter ending logic to ResolveEncounter() in Encounter.cs
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

    private static AttackModifierDict ApplyAttackModifiers(AttackModifierDict modifiers)
    {
        modifiers.TryGetValue(AttackModifier.CritChance, out int toCritModifier);
        modifiers.TryGetValue(AttackModifier.CritDamage, out int critDamageModifier);
        modifiers.TryGetValue(AttackModifier.HitChance, out int toHitModifier);
        modifiers.TryGetValue(AttackModifier.HitDamage, out int hitDamageModifier);

        return new() {
            { AttackModifier.CritChance, Math.Min(Math.Max(Attack.Probabilities[AttackModifier.CritChance] + toCritModifier, MIN_CHANCE), MAX_CHANCE) } ,
            { AttackModifier.CritDamage, Math.Max(Attack.Probabilities[AttackModifier.CritDamage] + critDamageModifier, MIN_ATTACK_DAMAGE) },
            { AttackModifier.HitChance, Math.Min(Math.Max(Attack.Probabilities[AttackModifier.HitChance] + toHitModifier, MIN_CHANCE), MAX_CHANCE) },
            { AttackModifier.HitDamage, Math.Max(Attack.Probabilities[AttackModifier.HitDamage] + hitDamageModifier, MIN_ATTACK_DAMAGE) },
        };
    }
}

public enum AttackModifier
{
    CritChance,
    CritDamage,
    HitChance,
    HitDamage,
}

public sealed class AttackAction : HHAction
{
    public readonly AttackModifierDict Probabilities = new() {
        { AttackModifier.CritChance, 95 } ,
        { AttackModifier.CritDamage, 2 },
        { AttackModifier.HitChance, 50 },
        { AttackModifier.HitDamage, 1 },
    };

    public Action<Encounter, AttackModifierDict, Random> TakeAction { get; set; }
}
