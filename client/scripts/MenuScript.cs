using Godot;
using System;
using System.Text;

using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

public class MenuScript : Control
{
	public override void _Ready()
	{
		HTTPRequest httpGetGameState = GetNode<HTTPRequest>("HTTPGetGameState");

		httpGetGameState.Connect("request_completed", this, "OnRequestCompleted");
		httpGetGameState.Request("https://localhost:7111/GameState");
	}

	private void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
	{
		if (response_code < 200 || response_code > 299)
		{
			return;
		}

		GameState gameState = HHJsonSerializer.Deserialize<GameState>(body);

		RichTextLabel worldNameNode = GetNode<RichTextLabel>("WorldName");

		worldNameNode.Text = gameState.World.WorldName;
	}
}
