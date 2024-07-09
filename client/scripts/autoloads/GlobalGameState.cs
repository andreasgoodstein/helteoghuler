using AutoMapper;
using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;

namespace HelteOgHulerClient;

public class GlobalGameState : Node
{
	private static Mapper _mapper;

	private static GameState _gameState;

	private static PubSub<GameState> _gameStateChannel;

	public GlobalGameState()
	{
		_gameState = new GameState() { };

		_gameStateChannel = new PubSub<GameState>();

		_mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<GameState, GameState>()));
	}

	public static GameState Get()
	{
		return _gameState;
	}

	public static void Register(ISubscriber<GameState> subscriber)
	{
		_gameStateChannel.Register(subscriber);
	}

	public static void Unregister(ISubscriber<GameState> subscriber)
	{
		_gameStateChannel.Unregister(subscriber);
	}

	public static void Update(GameState newGameState)
	{
		_gameState = _mapper.Map(newGameState, _gameState);

		_gameStateChannel.Publish(_gameState);
	}

	public static void Update(IApplicable applicable)
	{
		var playerId = GameStateHelper.GetPlayer(_gameState)?.Id;

		applicable.ApplyToGameState(ref _gameState, playerId);

		_gameStateChannel.Publish(_gameState);
	}
}
