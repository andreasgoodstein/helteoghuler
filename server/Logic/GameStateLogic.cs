using HelteOgHulerServer.Models;
using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerServer.Services;

namespace HelteOgHulerServer.Logic;


public class GameStateLogic
{

    private GameState _globalGameState = new GameState
    {
        Player = new Player
        {
            Id = Guid.NewGuid(),
            Inn = new Inn
            {
                Chest = new Chest()
                {
                    Gold = 0,
                    Id = Guid.NewGuid()
                },
                Id = Guid.NewGuid(),
                Name = "Jagtstuen"
            },
            Name = "TestPlayer"
        },
        World = new WorldState
        {
            WorldName = "Konia"
        }
    };

    private EventService _eventService;

    public GameStateLogic(EventService eventService)
    {
        _eventService = eventService;

        _globalGameState = RegenerateGameState().Result;
    }

    public GameState GetGameState() => _globalGameState;

    public async Task<GameState> RegenerateGameState()
    {
        (await _eventService.GetAsyncAsc()).ForEach(gameEvent =>
        {
            gameEvent.ApplyToGameState(_globalGameState);
        });

        return _globalGameState;
    }

    public GameState UpdateGameState(IEvent gameEvent)
    {
        gameEvent.ApplyToGameState(_globalGameState);

        return _globalGameState;
    }
}
