using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using System;

public class LoginMenuScript : Control, ISubscriber<GameState>
{
	private LineEdit LineEdit;

	public override void _Ready()
	{
		LineEdit = GetNode<LineEdit>("StartAdventure/UserNameInput");

		GetNode<Button>("StartAdventure").Connect("pressed", this, "StartAdventurePressed");

		GlobalGameState.Register(this);

		var loginName = GetNode<Settings>("/root/Settings").LoginName;

		if (!String.IsNullOrWhiteSpace(loginName))
		{
			LineEdit.Text = loginName;
		}
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public string GetId()
	{
		return Filename + Name;
	}

	private void StartAdventurePressed()
	{
		if (String.IsNullOrWhiteSpace(LineEdit.Text))
		{
			return;
		}

		GetNode<Settings>("/root/Settings").LoginName = LineEdit.Text;

		GetNode<Server>("/root/Server").RefreshGameState(this);
	}

	public void Message(GameState message)
	{
		if (message?.PrivatePlayerDict?.Count < 1)
		{
			GetTree().ChangeScene("res://scenes/NewPlayerScene.tscn");
		}
		else
		{
			GetTree().ChangeScene("res://scenes/InnScene.tscn");
		}
	}
}
