using Godot;
using System;
using System.Text;

using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

public class MenuScript : Control
{
	public override void _Ready()
	{
		HTTPRequest httpGetGameState = GetNode<HTTPRequest>("/root/Network");

		httpGetGameState.Connect("request_completed", this, "OnRequestCompleted");
		httpGetGameState.Request("http://localhost:7111/GameState");

		GetNode<Button>("GoToAdventure").Connect("pressed", this, "GoToAdventurePressed");
	}

	private void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
	{
		if (response_code < 200 || response_code > 299)
		{
			GD.Print("Network: Could not get GameState");
			return;
		}

		GameState gameState = HHJsonSerializer.Deserialize<GameState>(body);

		GlobalGameState.Update(gameState);

		RichTextLabel worldNameNode = GetNode<RichTextLabel>("WorldName");

		worldNameNode.Text = gameState.World.WorldName;
	}

	private void GoToAdventurePressed()
	{
		GetTree().ChangeScene("res://scenes/AdventureStatsScene.tscn");
	}
}
