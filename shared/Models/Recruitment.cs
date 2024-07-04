using HelteOgHulerShared.Interfaces;
using System;

namespace HelteOgHulerShared.Models;

public class Recruitment : IApplicable
{
    public Guid PlayerId { get; set; }

    public Guid HeroId { get; set; }

    public void ApplyToGameState(ref GameState gameState, Guid? id)
    {
        var inn = gameState.PrivatePlayerDict[(Guid)id]?.Inn;
        var hero = inn?.HeroRecruits?[HeroId];

        inn?.HeroRoster.Add(hero.Id, hero);
        inn?.HeroRecruits.Remove(hero.Id);
    }

    public void RemoveFromGameState(ref GameState gameState, Guid? id)
    {
        var inn = gameState.PrivatePlayerDict[(Guid)id]?.Inn;
        var hero = inn?.HeroRoster?[HeroId];

        inn?.HeroRecruits.Add(hero.Id, hero);
        inn?.HeroRoster.Remove(hero.Id);
    }
}
