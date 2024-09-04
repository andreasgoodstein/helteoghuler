using HelteOgHulerShared.Interfaces;
using System.Runtime.Serialization;

#if (NET6_0_OR_GREATER)
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
#endif

namespace HelteOgHulerShared.Models;

public class HHAction
{
    public ActionName Name { get; set; }
    public string Outcome { get; set; }
    public ActionTarget Target { get; set; }
    public double Probability { get; set; }
    public double CritProbability { get; set; }


#if (NET6_0_OR_GREATER)
    [BsonIgnore]
    [JsonIgnore]
#endif
    [IgnoreDataMember]
    public Action<Encounter, Random> TakeAction { get; set; }
}


public static class Actions
{
    private static readonly HHAction Attack = new()
    {
        Probability = .5,
        CritProbability = .95,
        Name = ActionName.Attack,
        Outcome = "ACTOR takes a swing at TARGET. (Success: 50 | Crit: 95)",
        Target = ActionTarget.Enemy,
        TakeAction = (Encounter encounter, Random random) =>
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

            encounter.ActionLog.Add(Attack.Outcome.Replace("ACTOR", encounter.CurrentlyActing.Name).Replace("TARGET", target.Name));

            double roll = random.NextDouble();
            bool doesAttackHit = roll.CompareTo(Attack.Probability) >= 0;
            bool doesAttackCrit = roll.CompareTo(Attack.CritProbability) >= 0;

            if (!doesAttackHit)
            {
                encounter.ActionLog.Add($"They roll a {Math.Round(roll * 100)} and miss!");
                return;
            }

            if (doesAttackCrit)
            {
                encounter.ActionLog.Add($"They roll a {Math.Round(roll * 100)} and critically hit! Doing 2 damage.");
                target.HP -= 2;
            }
            else
            {
                encounter.ActionLog.Add($"They roll a {Math.Round(roll * 100)} and hit! Doing 1 damage.");
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

    // private static readonly HHAction Dodge = new()
    // {
    //     Name = ActionName.Dodge,
    //     Outcome = "ACTOR prepares to dodge the next attack.",
    //     Target = ActionTarget.Self,
    //     TakeAction = (Encounter encounter, Random random) =>
    //     {
    //         encounter.ActionLog.Add(Dodge.Outcome.Replace("ACTOR", encounter.CurrentlyActing.Name));

    //         // TODO: Figure out setting (and clearing) abilities dynamically
    //     }
    // };

    public static readonly Dictionary<ActionName, HHAction> DefaultActions = new()
    {
        {ActionName.Attack, Attack},
        // {ActionName.Dodge, Dodge},
    };
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
