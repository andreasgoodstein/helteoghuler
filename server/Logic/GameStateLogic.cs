using System.Text.Json;
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

    // Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production";

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
        _globalGameState.CurrentTime = DateTime.UtcNow;

        GameState gameState = JsonSerializer.Deserialize<GameState>(
            JsonSerializer.Serialize<GameState>(_globalGameState)
        )!;

        if (!gameState.PrivatePlayerDict.ContainsKey(playerId))
        {
            gameState.PrivatePlayerDict.Clear();
            return gameState;
        }

        var privatePlayer = gameState.GetPlayer(playerId);

        gameState.PrivatePlayerDict.Clear();
        gameState.PrivatePlayerDict[playerId] = privatePlayer;

        return gameState;
    }

    public async Task<GameState> RegenerateGameState()
    {
        (await _eventService.GetAsyncAsc()).ForEach(gameEvent =>
        {
            gameEvent.ApplyToGameState(ref _globalGameState, null);
        });

        _globalGameState.CurrentTime = DateTime.UtcNow;

        await _gameStateService.CreateAsync(_globalGameState);

        return _globalGameState;
    }

    public GameState UpdateGameState(IApplicable gameEvent)
    {
        gameEvent.ApplyToGameState(ref _globalGameState, null);

        _globalGameState.CurrentTime = DateTime.UtcNow;

        return _globalGameState;
    }
}
