using System;
using System.Linq;
using Godot;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerClient.Utilities;

public static class GameStateHelper
{
	public static Player GetPlayer(GameState gameState)
	{
		var playerEnumerator = gameState?.PrivatePlayerDict?.Values;

		return playerEnumerator.First();
	}
}
