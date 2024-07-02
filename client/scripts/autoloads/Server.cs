using Godot;
using HelteOgHulerClient.Services;

namespace HelteOgHulerClient;

public class Server : Node
{
	private AdventureService _adventureService;
	private GameStateService _gameStateService;
	private PlayerService _playerService;

	public Server()
	{
		_adventureService = new AdventureService();
		_gameStateService = new GameStateService();
		_playerService = new PlayerService();
	}

	public async void CreateNewPlayer(Node httpRequestParent, string playerName, string innName)
	{
		await _playerService.CreateNewPlayer(httpRequestParent, playerName, innName);
	}

	public async void RefreshGameState(Node httpRequestParent)
	{
		await _gameStateService.RefreshGameState(httpRequestParent);
	}

	public async void StartAdventure(Node httpRequestParent)
	{
		await _adventureService.StartAdventure(httpRequestParent);
	}
}
