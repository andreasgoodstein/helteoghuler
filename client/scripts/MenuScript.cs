using Godot;
using System;
using System.Text;

using HelteOgHulerClient;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

public class MenuScript : Control, ISubscriber<GameState>
{
	private Server server;

	public override void _Ready()
	{
		GlobalGameState.Listen(this);

		GetNode<Button>("GoToAdventure").Connect("pressed", this, "GoToAdventurePressed");

		GetNode<Server>("/root/Server").RefreshGameState(this);
	}

	public void Message(GameState gameState)
	{
		GetNode<RichTextLabel>("WorldName").Text = gameState.World.WorldName;
	}

	private void GoToAdventurePressed()
	{
		GetTree().ChangeScene("res://scenes/AdventureStatsScene.tscn");
	}
}
