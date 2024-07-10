using Godot;
using HelteOgHulerClient;
using System;

public class NewPlayerScript : Control
{
	private LineEdit InnName;
	private LineEdit PlayerName;

	public override void _Ready()
	{
		InnName = GetNode<LineEdit>("%InnNameInput");
		PlayerName = GetNode<LineEdit>("%PlayerNameInput");

		GetNode<Button>("%GoToInn").Connect("pressed", this, "GoToInnPressed");
	}

	private async void GoToInnPressed()
	{
		if (String.IsNullOrWhiteSpace(InnName.Text) || String.IsNullOrWhiteSpace(PlayerName.Text))
		{
			return;
		}

		await GetNode<Server>("/root/Server").CreateNewPlayer(this, InnName.Text, PlayerName.Text);

		GetTree().ChangeScene("res://scenes/InnScene.tscn");
	}
}
