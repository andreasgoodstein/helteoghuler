using AutoMapper;
using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
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

	public static GameState Update(GameState newGameState)
	{
		_gameStateChannel.Publish(newGameState);

		_gameState = _mapper.Map(newGameState, _gameState);

		return _gameState;
	}
}
