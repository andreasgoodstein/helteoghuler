using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerShared.Models;

public class AdventureScript : Control, ISubscriber<GameState>
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

	}

	public string GetId()
	{
		return Filename + Name;
	}
}
