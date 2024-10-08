using HelteOgHulerShared.Models;

namespace HelteOgHulerShared.Utilities;

public static class GameStateHelper
{
	public static Player GetPlayer(this GameState gameState)
	{
		return gameState?.PrivatePlayerDict?.Values?.FirstOrDefault();
	}
	public static Player GetPlayer(this GameState gameState, Guid playerId)
	{
		return gameState?.PrivatePlayerDict?[playerId];
	}

	public static bool IsResting(this GameState gameState)
	{
		return GetPlayer(gameState)?.RestUntil > DateTime.UtcNow;
	}
}
