using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using System;

public class NewPlayerScript : Control, ISubscriber<GameState>
{
	private LineEdit InnName;
	private LineEdit PlayerName;

	public override void _Ready()
	{
		InnName = GetNode<LineEdit>("InnNameInput");
		PlayerName = GetNode<LineEdit>("PlayerNameInput");

		GetNode<Button>("GoToInn").Connect("pressed", this, "GoToInnPressed");
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	private void GoToInnPressed()
	{
		if (String.IsNullOrWhiteSpace(InnName.Text) || String.IsNullOrWhiteSpace(PlayerName.Text))
		{
			return;
		}

		GetNode<Server>("/root/Server").CreateNewPlayer(this, InnName.Text, PlayerName.Text);
	}

	public void Message(GameState message)
	{
		if (message?.PrivatePlayerDict?.Count > 0)
		{
			GetTree().ChangeScene("res://scenes/InnScene.tscn");
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
