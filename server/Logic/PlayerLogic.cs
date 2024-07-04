using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;

public class PlayerLogic
{
    private readonly InnLogic _innLogic;

    public PlayerLogic(InnLogic innLogic)
    {
        _innLogic = innLogic;
    }

    public Player CreatePlayer(GameState gameState, Guid playerId, string innName, string playerName)
    {
        if (gameState.PrivatePlayerDict.ContainsKey(playerId))
        {
            throw new InvalidDataException("This deed has already been claimed.");
        }

        return GeneratePlayer(playerId, innName, playerName);
    }

    private Player GeneratePlayer(Guid playerId, string innName, string playerName)
    {
        return new Player
        {
            Id = playerId,
            Inn = _innLogic.GenerateInn(innName),
            Name = playerName,
        };
    }
}
