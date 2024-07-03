using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using HelteOgHulerClient.Utilities;

public class InnScript : Control, ISubscriber<GameState>
{
	public override void _Ready()
	{
		// Message(GlobalGameState.Get());

		// GlobalGameState.Register(this);
	}

	public override void _ExitTree()
	{
		// GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		// var player = GameStateHelper.GetPlayer(gameState);
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
