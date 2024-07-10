using System.Threading.Tasks;
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

	public Task CreateNewPlayer(Node httpRequestParent, string innName, string playerName)
	{
		return _playerService.CreateNewPlayer(httpRequestParent, innName, playerName);
	}

	public Task RefreshGameState(Node httpRequestParent)
	{
		return _gameStateService.RefreshGameState(httpRequestParent);
	}

	public Task StartAdventure(Node httpRequestParent)
	{
		return _adventureService.StartAdventure(httpRequestParent);
	}
}
