using HelteOgHulerServer.Models;
using HelteOgHulerServer.Interfaces;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;

namespace HelteOgHulerServer.Logic;

public class GameStateLogic
{

    private GameState _globalGameState = new GameState
    {
        CurrentTime = DateTime.UtcNow,
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
        World = new World
        {
            Name = "Konia"
        }
    };

    private EventService _eventService;

    public GameStateLogic(EventService eventService)
    {
        _eventService = eventService;

        _globalGameState = RegenerateGameState().Result;
    }

    public GameState Get()
    {
        _globalGameState.CurrentTime = DateTime.UtcNow;

        return _globalGameState;
    }

    public async Task<GameState> RegenerateGameState()
    {
        (await _eventService.GetAsyncAsc()).ForEach(gameEvent =>
        {
            gameEvent.ApplyToGameState(ref _globalGameState);
        });

        _globalGameState.CurrentTime = DateTime.UtcNow;

        return _globalGameState;
    }

    public GameState UpdateGameState(IEvent gameEvent)
    {
        gameEvent.ApplyToGameState(ref _globalGameState);

        _globalGameState.CurrentTime = DateTime.UtcNow;

        return _globalGameState;
    }
}
