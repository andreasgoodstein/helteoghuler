using HelteOgHulerServer.Services;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerServer.Logic;

public class GameStateLogic
{
    private GameState _globalGameState = new()
    {
        CurrentTime = DateTime.UtcNow,
        PrivatePlayerDict = [],
        PublicPlayerDict = [],
        World = new World { Name = "East Island" },
    };

    public bool SaveStateToDatabase = true;

    private readonly EventService _eventService;
    private readonly GameStateService _gameStateService;

    public GameStateLogic(EventService eventService, GameStateService gameStateService)
    {
        _eventService = eventService;
        _gameStateService = gameStateService;

        _globalGameState = RegenerateGameState().Result;

        _globalGameState.CurrentTime = DateTime.UtcNow;
    }

    public GameState Get()
    {
        _globalGameState.CurrentTime = DateTime.UtcNow;

        return _globalGameState;
    }

    /// <summary>
    /// Returns the current GameState, with PrivatePlayerDict only containing the
    /// given playerId value
    /// </summary>
    public GameState Get(Guid playerId)
    {
        GameState gameState = new()
        {
            CurrentTime = DateTime.UtcNow,
            PrivatePlayerDict = [],
            PublicPlayerDict = _globalGameState.PublicPlayerDict,
            World = _globalGameState.World,
        };

        gameState.PrivatePlayerDict.Add(playerId, _globalGameState.PrivatePlayerDict[playerId]);

        return gameState;
    }

    public async Task<GameState> RegenerateGameState()
    {
        (await _eventService.GetAsyncAsc()).ForEach(gameEvent =>
        {
            gameEvent.ApplyToGameState(ref _globalGameState, null);
        });

        _globalGameState.CurrentTime = DateTime.UtcNow;

        await _gameStateService.PersistGameState(_globalGameState);

        return _globalGameState;
    }

    public GameState UpdateGameState(IApplicable gameEvent)
    {
        gameEvent.ApplyToGameState(ref _globalGameState, null);

        _globalGameState.CurrentTime = DateTime.UtcNow;

        return _globalGameState;
    }
}
