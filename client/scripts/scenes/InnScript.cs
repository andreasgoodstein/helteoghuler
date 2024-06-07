using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class InnScript : Control, ISubscriber<GameState>
{
	private KeyValueLabel InnKeeper;

	public override void _Ready()
	{
		GlobalGameState.Register(this);

		GameState gameState = GlobalGameState.Get();

		InnKeeper = GetNode<KeyValueLabel>("InnKeeper");
		InnKeeper.Set(gameState?.Player?.Name);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		if (gameState?.Player?.Name != null)
		{
			InnKeeper.Set(gameState.Player.Name);
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
