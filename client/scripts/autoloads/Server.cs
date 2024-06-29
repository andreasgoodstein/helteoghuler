using Godot;
using HelteOgHulerClient.Services;
using HelteOgHulerClient.Utilities;

namespace HelteOgHulerClient;

public class Server : Node
{
	private AdventureService _adventureService;
	private GameStateService _gameStateService;

	public Server()
	{
		_adventureService = new AdventureService();
		_gameStateService = new GameStateService();
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
