using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class AdventureStatsScript : Control, ISubscriber<GameState>
{
	private KeyValueLabel TotalAdventures;
	private KeyValueLabel GoldTracker;

	public override void _Ready()
	{
		GlobalGameState.Register(this);

		GameState gameState = GlobalGameState.Get();

		GoldTracker = GetNode<KeyValueLabel>("Container/GoldTracker");
		GoldTracker.Set(gameState?.Player?.Inn?.Chest?.Gold ?? 0);

		TotalAdventures = GetNode<KeyValueLabel>("Container/TotalAdventuresTracker");
		TotalAdventures.Set(gameState?.World?.TotalAdventures ?? 0);
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
