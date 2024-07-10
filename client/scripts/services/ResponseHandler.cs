using Godot;
using HelteOgHulerShared.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerClient.Services;

public static class ResponseHandler
{
	public static void HandleGameStateResponse(byte[] body)
	{
		GameState newGameState = HHJsonSerializer.Deserialize<GameState>(body);

		if (newGameState == null)
		{
			GD.PrintErr("Network: GameStateResponse empty.");
			return;
		}

		GlobalGameState.Update(newGameState);
	}

	public static void HandleGameStateResponse<T>(byte[] body) where T : IApplicable
	{
		T responseObject = HHJsonSerializer.Deserialize<T>(body);

		if (responseObject == null)
		{
			GD.PrintErr("Network: ErrorResponse empty.");
			return;
		}

		GlobalGameState.Update(responseObject);
	}
}
