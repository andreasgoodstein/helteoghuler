using HelteOgHulerServer.Utilities;
using HelteOgHulerShared.Models;

namespace HelteOgHulerServer.Logic;

public class HeroLogic
{
    private NameUtility _nameUtility;

    public HeroLogic(NameUtility nameUtility)
    {
        _nameUtility = nameUtility;
    }

    public Hero GenerateHero()
    {
        return new Hero
        {
            Id = Guid.NewGuid(),
            Name = _nameUtility.GenerateName(),
            Price = 200
        };
    }
}
