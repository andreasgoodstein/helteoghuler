using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using System;

public class MenuScript : Control, ISubscriber<GameState>
{
	public override void _Ready()
	{
		GetNode<Button>("GoToAdventure").Connect("pressed", this, "GoToAdventurePressed");

		GetNode<Server>("/root/Server").RefreshGameState(this);

		Message(GlobalGameState.Get());

		GlobalGameState.Register(this);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		if (!String.IsNullOrWhiteSpace(gameState?.World?.Name))
		{
			GetNode<Label>("Title/WorldName").Text = gameState.World.Name;
		}
	}

	private void GoToAdventurePressed()
	{
		GetTree().ChangeScene("res://scenes/AdventureScene.tscn");
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
