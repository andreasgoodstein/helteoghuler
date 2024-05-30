using Godot;
using HelteOgHulerClient;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerShared.Models;

public class AdventureStatsScript : Control, ISubscriber<GameState>
{
	private Label TotalAdventures;

	public override void _Ready()
	{
		GlobalGameState.Register(this);

		TotalAdventures = GetNode<Label>("TotalAdventures/Value");
		TotalAdventures.Text = (GlobalGameState.Get()?.World?.TotalAdventures ?? 0).ToString();
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		if (gameState?.World?.TotalAdventures != null)
		{
			TotalAdventures.Text = gameState.World.TotalAdventures.ToString();
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
