using System.Threading.Tasks;
using Godot;
using HelteOgHulerClient.Services;

namespace HelteOgHulerClient;

public class Server : Node
{
	private AdventureService _adventureService;
	private GameStateService _gameStateService;
	private InnService _innService;
	private PlayerService _playerService;

	public Server()
	{
		_adventureService = new AdventureService();
		_gameStateService = new GameStateService();
		_innService = new InnService();
		_playerService = new PlayerService();
	}

	public Task CreateNewPlayer(Node httpRequestParent, string innName, string playerName)
	{
		return _playerService.CreateNewPlayer(httpRequestParent, innName, playerName);
	}

	public Task RecruitHero(Node httpRequestParent, string heroId)
	{
		return _innService.RecruitHero(httpRequestParent, heroId);
	}

	public Task RefreshGameState(Node httpRequestParent)
	{
		return _gameStateService.RefreshGameState(httpRequestParent);
	}

	public Task<string> StartAdventure(Node httpRequestParent)
	{
		return _adventureService.StartAdventure(httpRequestParent);
	}
}
