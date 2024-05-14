using Godot;
using System;
using System.Text;

public class Menu : Control
{
	public override void _Ready()
	{
		GetNode("HTTPGetGameState").Connect("request_completed", this, "OnRequestCompleted");

		HTTPRequest httpGetGameState = GetNode<HTTPRequest>("HTTPGetGameState");
		httpGetGameState.Request("https://localhost:7111/GameState");
	}

	public void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
	{
		JSONParseResult json = JSON.Parse(Encoding.UTF8.GetString(body));
		GD.Print(json.Result);
	}
}
