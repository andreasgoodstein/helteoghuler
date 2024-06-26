using Godot;

public class MenuScript : Control
{
	public override void _Ready()
	{
		GetNode<Button>("StartAdventure").Connect("pressed", this, "StartAdventurePressed");
	}

	private void StartAdventurePressed()
	{
		// TODO: Try login and choose scene
		// GetNode<Server>("/root/Server").RefreshGameState(this);
		GetTree().ChangeScene("res://scenes/NewPlayerScene.tscn");
	}
}
