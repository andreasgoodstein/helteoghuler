using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class InnScript : Control, ISubscriber<GameState>
{
	private KeyValueLabel InnKeeper;

	public override void _Ready()
	{
		InnKeeper = GetNode<KeyValueLabel>("InnKeeper");

		Message(GlobalGameState.Get());

		GlobalGameState.Register(this);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		var player = gameState?.PrivatePlayerDict?.Values?.GetEnumerator().Current;

		if (player?.Name != null)
		{
			InnKeeper.Set(player.Name);
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
