using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;

public class PlayerLogic
{
    private readonly EventService _eventService;

    private readonly GameStateLogic _gameStateLogic;

    public PlayerLogic(EventService eventService, GameStateLogic gameStateLogic)
    {
        _eventService = eventService;
        _gameStateLogic = gameStateLogic;
    }

    public async Task CreatePlayer(Guid playerId, string playerName, string innName)
    {
        var newPlayerEvent = new NewPlayerEvent
        {
            CreatedAt = DateTime.UtcNow,
            Player = new Player
            {
                Id = playerId,
                Inn = new Inn
                {
                    Chest = new Chest
                    {
                        Gold = 0,
                        Id = Guid.NewGuid(),
                    },
                    Id = Guid.NewGuid(),
                    Name = innName,
                },
                Name = playerName,
            },
        };

        await _eventService.CreateAsync(newPlayerEvent);

        _gameStateLogic.UpdateGameState(newPlayerEvent);
    }
}
