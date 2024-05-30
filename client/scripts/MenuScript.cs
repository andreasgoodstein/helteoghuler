using Godot;
using System;

using HelteOgHulerClient;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerShared.Models;

public class MenuScript : Control, ISubscriber<GameState>
{
	public override void _Ready()
	{
		GlobalGameState.Register(this);

		GetNode<Button>("GoToAdventure").Connect("pressed", this, "GoToAdventurePressed");

		GetNode<Server>("/root/Server").RefreshGameState(this);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		if (!String.IsNullOrWhiteSpace(gameState?.World?.WorldName))
		{
			GetNode<RichTextLabel>("WorldName").Text = gameState.World.WorldName;
		}
	}

	private void GoToAdventurePressed()
	{
		GetTree().ChangeScene("res://scenes/AdventureStatsScene.tscn");
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
