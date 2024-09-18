using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class AdventureScript : Control, ISubscriber<GameState>
{
	public override void _Ready()
	{
		Message(GlobalGameState.Get());

		// GlobalGameState.Register(this);
	}

	public override void _ExitTree()
	{
		// GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		var player = GameStateHelper.GetPlayer(gameState);

		GetNode<Label>("%Message").Text = AdventureHelper.GetAdventureText(player.LatestAdventure);
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
