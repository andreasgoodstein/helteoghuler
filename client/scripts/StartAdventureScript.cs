using Godot;
using HelteOgHulerClient;

public class StartAdventureScript : Button
{
	public override void _Ready()
	{
		GetNode<Button>("../StartAdventureButton").Connect("pressed", this, "StartAdventurePressed");
	}

	private void StartAdventurePressed()
	{
		GetNode<Server>("/root/Server").StartAdventure(this);
	}
}
