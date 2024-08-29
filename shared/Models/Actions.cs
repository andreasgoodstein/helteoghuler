using HelteOgHulerShared.Interfaces;
using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class HHAction
{
    public ActionName Name { get; set; }
    public string Outcome { get; set; }
    public ActionTarget Target { get; set; }
    public Action<Encounter, Random> TakeAction { get; set; }
    public double Probability { get; set; }
}


public static class Actions
{
    private static HHAction Attack = new HHAction()
    {
        Probability = .5,
        Name = ActionName.Attack,
        Outcome = "ACTOR takes a swing at TARGET.",
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

            double roll = random.NextDouble();
            bool doesAttackHit = roll > Attack.Probability;

            encounter.ActionLog.Add(Attack.Outcome.Replace("ACTOR", encounter.CurrentlyActing.Name).Replace("TARGET", target.Name));

            if (!doesAttackHit)
            {
                encounter.ActionLog.Add($"They roll a {Math.Round(roll * 100)} and miss!");
                return;
            }

            encounter.ActionLog.Add($"They roll a {Math.Round(roll * 100)} and hit! Doing 1 damage.");

            target.HP -= 1;

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

    // private static HHAction Dodge = new HHAction()
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


    public static Dictionary<ActionName, HHAction> DefaultActions = new Dictionary<ActionName, HHAction>()
    {
        {ActionName.Attack, Attack},
        // {ActionName.Dodge, Dodge},
    };
}

public enum ActionName
{
    Attack = 0,
    // Dodge = 1,
}

public enum ActionTarget
{
    Self = 0,
    Ally = 1,
    Enemy = 2
}
