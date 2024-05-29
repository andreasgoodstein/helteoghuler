using AutoMapper;
using HelteOgHulerShared.Models;

namespace HelteOgHulerClient;

public class GameStateStore
{
    private Mapper _mapper;

    private GameState _gameState;

    private PubSub<GameState> _gameStateChannel;

    public GameStateStore()
    {
        _gameState = new GameState() { };

        _gameStateChannel = new PubSub<GameState>();

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<GameState, GameState>()));
    }

    public GameState Get()
    {
        return _gameState;
    }

    public void Set(GameState newGameState)
    {
        _gameStateChannel.Publish(newGameState);

        _gameState = _mapper.Map<GameState, GameState>(newGameState, _gameState);
    }
}
