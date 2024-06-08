using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class AdventureScript : Control, ISubscriber<GameState>
{
	private KeyValueLabel GoldTracker;
	private KeyValueLabel MessageTracker;

	public override void _Ready()
	{
		GoldTracker = GetNode<KeyValueLabel>("Container/GoldTracker");
		MessageTracker = GetNode<KeyValueLabel>("Container/MessageTracker");

		Message(GlobalGameState.Get());

		GlobalGameState.Register(this);
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

		MessageTracker.Set(gameState.ErrorMessage);
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
