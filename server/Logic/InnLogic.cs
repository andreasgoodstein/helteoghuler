using HelteOgHulerShared.Models;

namespace HelteOgHulerServer.Logic;

public class InnLogic
{
    private readonly HeroLogic _heroLogic;

    public InnLogic(HeroLogic heroLogic)
    {
        _heroLogic = heroLogic;
    }

    public Inn GenerateInn(string innName)
    {
        return new Inn
        {
            Chest = new Chest
            {
                Gold = 0,
                Id = Guid.NewGuid(),
            },
            HeroRecruits = new List<Hero> { _heroLogic.GenerateHero() },
            Id = Guid.NewGuid(),
            Name = innName,
        };
    }
}
