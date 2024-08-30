using HelteOgHulerServer.Utilities;
using HelteOgHulerShared.Models;

namespace HelteOgHulerServer.Logic;

public class HeroLogic(NameUtility nameUtility)
{
    private readonly NameUtility _nameUtility = nameUtility;

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
