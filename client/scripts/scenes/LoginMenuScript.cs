using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class LoginMenuScript : Control, ISubscriber<GameState>
{
	private LineEdit LineEdit;

	public override void _Ready()
	{
		LineEdit = GetNode<LineEdit>("UserNameInput");

		GetNode<Button>("StartAdventure").Connect("pressed", this, "StartAdventurePressed");

		GlobalGameState.Register(this);
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

		// TODO: Try login and choose scene
		// GetNode<Server>("/root/Server").RefreshGameState(this);
		GetTree().ChangeScene("res://scenes/NewPlayerScene.tscn");
	}

	public void Message(GameState message)
	{
		throw new System.NotImplementedException();
	}
}
