using HelteOgHulerShared.Models;

namespace HelteOgHulerServer.Logic;

public class AdventureLogic(GameStateLogic gameStateLogic, InnLogic innLogic)
{
    private readonly GameStateLogic _gameStateLogic = gameStateLogic;
    private readonly InnLogic _innLogic = innLogic;

    private bool CanPlayerAdventureForth(Guid playerId)
    {
        return (_gameStateLogic.Get()?.PrivatePlayerDict[playerId]?.RestUntil ?? DateTime.UtcNow) <= DateTime.UtcNow;
    }

    public Adventure GenerateAdventure(Guid playerId)
    {
        if (!CanPlayerAdventureForth(playerId))
        {
            throw new InvalidOperationException("Your party needs more rest, and cannot venture forth yet.");
        }

        var party = _innLogic.GatherParty(playerId);

        Adventure adventure = new();

        adventure.ResolveAdventure(party);

        return adventure;
    }
}
