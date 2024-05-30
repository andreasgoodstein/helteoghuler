using Godot;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerClient;

public class Server : Node
{
	private RequestNode refreshGameStateNode;
	private RequestNode startAdventureNode;

	public void RefreshGameState(Node httpRequestParent)
	{
		if (refreshGameStateNode != null)
		{
			refreshGameStateNode.Clean();
		}

		refreshGameStateNode = new RequestNode(httpRequestParent);

		refreshGameStateNode.Request.Connect("request_completed", this, "OnRefreshGameStateResponse");
		refreshGameStateNode.Request.Request("http://localhost:7111/GameState");
	}

	private void OnRefreshGameStateResponse(int result, int response_code, string[] headers, byte[] body)
	{
		if (response_code < 200 || response_code > 299)
		{
			GD.PrintErr("Network: Could not get GameState");
			return;
		}

		HandleGameStateResponse(body, refreshGameStateNode);
	}

	public void StartAdventure(Node httpRequestParent)
	{
		if (startAdventureNode != null)
		{
			startAdventureNode.Clean();
		}

		startAdventureNode = new RequestNode(httpRequestParent);

		startAdventureNode.Request.Connect("request_completed", this, "OnStartAdventureResponse");
		startAdventureNode.Request.Request("http://localhost:7111/Adventure/Start");
	}

	private void OnStartAdventureResponse(int result, int response_code, string[] headers, byte[] body)
	{
		if (response_code < 200 || response_code > 299)
		{
			GD.PrintErr("Network: Could not start Adventure");
			return;
		}

		HandleGameStateResponse(body, startAdventureNode);
	}

	private void HandleGameStateResponse(byte[] body, RequestNode usedNode)
	{
		GameState gameState = HHJsonSerializer.Deserialize<GameState>(body);

		GlobalGameState.Update(gameState);

		usedNode.Clean();
		usedNode = null;
	}
}
