using HelteOgHulerShared.Interfaces;
using System.Collections.Generic;
using System;

namespace HelteOgHulerShared.Models;

public class Recruitment : IApplicable
{
    public Guid PlayerId { get; set; }

    public Guid HeroId { get; set; }

    public void ApplyToGameState(ref GameState gameState, Guid? _)
    {
        var inn = gameState.PrivatePlayerDict[PlayerId]?.Inn;
        var hero = inn?.HeroRecruits?[HeroId.ToString()];

        if (inn?.HeroRoster != null)
        {
            inn.HeroRoster.Add(hero.Id.ToString(), hero);
        }
        else
        {
            inn.HeroRoster = new Dictionary<string, Hero> { { hero.Id.ToString(), hero } };
        }

        inn.Chest.Gold -= hero.Price;
        inn?.HeroRecruits?.Remove(hero.Id.ToString());
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? _)
    {
        var inn = gameState.PrivatePlayerDict[PlayerId]?.Inn;
        var hero = inn?.HeroRoster?[HeroId.ToString()];

        if (inn?.HeroRecruits != null)
        {
            inn.HeroRecruits.Add(hero.Id.ToString(), hero);
        }
        else
        {
            inn.HeroRecruits = new Dictionary<string, Hero> { { hero.Id.ToString(), hero } };
        }

        inn.Chest.Gold += hero.Price;
        inn?.HeroRoster?.Remove(hero.Id.ToString());
    }
}
