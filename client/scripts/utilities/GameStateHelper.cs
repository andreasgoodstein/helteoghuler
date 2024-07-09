using HelteOgHulerShared.Models;
using System.Linq;
using System;

namespace HelteOgHulerClient.Utilities;

public static class GameStateHelper
{
	public static Player GetPlayer(GameState gameState)
	{
		return gameState?.PrivatePlayerDict?.Values?.First();
	}

	public static bool IsResting(GameState gameState)
	{
		return GetPlayer(gameState)?.RestUntil > DateTime.UtcNow;
	}
}
