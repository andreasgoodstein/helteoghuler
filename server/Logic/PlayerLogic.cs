using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;

public class PlayerLogic
{
    private readonly EventService _eventService;
    private readonly GameStateLogic _gameStateLogic;
    private readonly InnLogic _innLogic;

    public PlayerLogic(EventService eventService, GameStateLogic gameStateLogic, InnLogic innLogic)
    {
        _eventService = eventService;
        _gameStateLogic = gameStateLogic;
        _innLogic = innLogic;
    }

    public async Task CreatePlayer(Guid playerId, string innName, string playerName)
    {
        var newPlayerEvent = new NewPlayerEvent
        {
            CreatedAt = DateTime.UtcNow,
            Player = GeneratePlayer(playerId, innName, playerName),
        };

        await _eventService.CreateAsync(newPlayerEvent);

        _gameStateLogic.UpdateGameState(newPlayerEvent);
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
