using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

public class PlayerLogic(InnLogic innLogic)
{
    private readonly InnLogic _innLogic = innLogic;

    public Player CreatePlayer(
        GameState gameState,
        Guid playerId,
        string innName,
        string playerName
    )
    {
        if (gameState.PrivatePlayerDict.ContainsKey(playerId))
        {
            throw new InvalidDataException("This deed has already been claimed.");
        }

        return GeneratePlayer(playerId, innName, playerName);
    }

    public void CanCompleteObjective(GameState gameState, Guid playerId, PlayerObjective objective)
    {
        var player = GameStateHelper.GetPlayer(gameState, playerId);

        if (player.ObjectivesCompleted.ContainsKey(objective))
        {
            throw new InvalidDataException("You have already done this mighty deed.");
        }
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
