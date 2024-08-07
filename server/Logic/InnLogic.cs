using HelteOgHulerShared.Models;

namespace HelteOgHulerServer.Logic;

public class InnLogic
{
    const ulong STARTING_GOLD = 200;

    private readonly GameStateLogic _gameStateLogic;
    private readonly HeroLogic _heroLogic;

    public InnLogic(GameStateLogic gameStateLogic, HeroLogic heroLogic)
    {
        _gameStateLogic = gameStateLogic;
        _heroLogic = heroLogic;
    }

    public Inn GenerateInn(string innName)
    {
        var newHeroRecruit = _heroLogic.GenerateHero();

        return new Inn
        {
            Chest = new Chest
            {
                Gold = STARTING_GOLD,
            },
            HeroRecruits = new Dictionary<string, Hero> { { newHeroRecruit.Id.ToString(), newHeroRecruit } },
            HeroRoster = [],
            Name = innName,
        };
    }

    public Recruitment RecruitHero(Guid playerId, Guid heroId)
    {
        var gameState = _gameStateLogic.Get();

        var inn = gameState.PrivatePlayerDict[playerId]?.Inn ?? throw new InvalidDataException("Innkeeper not found.");

        if (!inn.HeroRecruits.ContainsKey(heroId.ToString()))
        {
            throw new InvalidDataException("That Hero is not available.");
        }

        if (inn.Chest.Gold < inn.HeroRecruits[heroId.ToString()].Price)
        {
            throw new InvalidDataException("You cannot afford that Hero.");
        }

        return new Recruitment { HeroId = heroId, PlayerId = playerId };
    }
}
