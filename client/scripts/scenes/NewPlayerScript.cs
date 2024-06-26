using Godot;

public class NewPlayerScript : Control
{
	public override void _Ready()
	{
		GetNode<Button>("GoToInn").Connect("pressed", this, "GoToInnPressed");
	}

	private void GoToInnPressed()
	{
		// TODO: Try login and choose scene
		// GetNode<Server>("/root/Server").RefreshGameState(this);
		GetTree().ChangeScene("res://scenes/InnScene.tscn");
	}
}
