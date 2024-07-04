using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class WorldScript : Control, ISubscriber<GameState>
{
	private KeyValueLabelScript TotalAdventures;

	public override void _Ready()
	{
		TotalAdventures = GetNode<KeyValueLabelScript>("TotalAdventuresTracker");

		Message(GlobalGameState.Get());

		GlobalGameState.Register(this);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		if (gameState?.World?.TotalAdventures != null)
		{
			TotalAdventures.Set(gameState.World.TotalAdventures);
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
