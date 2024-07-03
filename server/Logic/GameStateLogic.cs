using System.Text.Json;
using HelteOgHulerServer.Interfaces;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerServer.Logic;

public class GameStateLogic
{

    private GameState _globalGameState = new GameState
    {
        CurrentTime = DateTime.UtcNow,
        PrivatePlayerDict = new Dictionary<Guid, Player>(),
        PublicPlayerDict = new Dictionary<Guid, PlayerPublic>(),
        World = new World
        {
            Name = "East Island"
        }
    };

    private EventService _eventService;

    public GameStateLogic(EventService eventService)
    {
        _eventService = eventService;

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

        GameState gameState = JsonSerializer.Deserialize<GameState>(JsonSerializer.Serialize<GameState>(_globalGameState))!;

        if (!gameState.PrivatePlayerDict.ContainsKey(playerId))
        {
            gameState.PrivatePlayerDict.Clear();
            return gameState;
        }

        var privatePlayer = gameState.PrivatePlayerDict[playerId];

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

        return _globalGameState;
    }

    public GameState UpdateGameState(IEvent gameEvent)
    {
        gameEvent.ApplyToGameState(ref _globalGameState, null);

        _globalGameState.CurrentTime = DateTime.UtcNow;

        return _globalGameState;
    }
}
