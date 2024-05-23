using HelteOgHulerServer.Models;
using HelteOgHulerServer.Models.Interfaces;
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
        if (gameEvent.Type == EventType.Adventure)
        {
            UpdateWithAdventure(gameEvent as AdventureEvent);
        }

        return _globalGameState;
    }

    private void UpdateWithAdventure(AdventureEvent adventureEvent)
    {
        _globalGameState.World.TotalAdventures += 1;
        _globalGameState.Player.Inn.Chest.Gold += adventureEvent.Adventure.Gold;
    }
}
