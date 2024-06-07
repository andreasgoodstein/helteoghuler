using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class AdventureScript : Control, ISubscriber<GameState>
{
	private KeyValueLabel GoldTracker;

	public override void _Ready()
	{
		GlobalGameState.Register(this);

		GameState gameState = GlobalGameState.Get();

		GoldTracker = GetNode<KeyValueLabel>("Container/GoldTracker");
		GoldTracker.Set(gameState?.Player?.Inn?.Chest?.Gold ?? 0);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		if (gameState?.Player?.Inn?.Chest?.Gold != null)
		{
			GoldTracker.Set(gameState.Player.Inn.Chest.Gold);
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
