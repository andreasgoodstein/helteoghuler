using Godot;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerClient;

public class Server : Node
{
	private RequestNode refreshGameStateNode;

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
			GD.Print("Network: Could not get GameState");
			return;
		}

		GameState gameState = HHJsonSerializer.Deserialize<GameState>(body);

		GlobalGameState.Update(gameState);

		refreshGameStateNode.Clean();
		refreshGameStateNode = null;
	}
}
