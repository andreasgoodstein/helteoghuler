using HelteOgHulerServer.Models;
using HelteOgHulerServer.Interfaces;
using HelteOgHulerShared.Models;

namespace HelteOgHulerServer.Logic;


public class GameStateLogic()
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

    public GameState GetGameState() => _globalGameState;

    public GameState RegenerateGameState()
    {
        // TODO: Read all events from database to reconstruct state
        // TODO: Store gamestate in database at intervals to only read new events when regenerating
        return _globalGameState;
    }

    public GameState UpdateGameState(IEvent gameEvent)
    {
        gameEvent.ApplyToGameState(_globalGameState);

        return _globalGameState;
    }
}
