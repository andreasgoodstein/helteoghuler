using Godot;
using HelteOgHulerClient;
using HelteOgHulerClient.Utilities;

public class TopMenuScript : VBoxContainer
{
	private Label PlayerInfo;

	public override void _Ready()
	{
		PlayerInfo = GetNode<Label>("PlayerInfoContainer/PlayerInfo");

		GetNode<Button>("SceneButtonContainer/AdventureButton").Connect("pressed", this, "GoToAdventure");
		GetNode<Button>("SceneButtonContainer/InnButton").Connect("pressed", this, "GoToInn");
		GetNode<Button>("SceneButtonContainer/WorldButton").Connect("pressed", this, "GoToWorld");

		var player = GameStateHelper.GetPlayer(GlobalGameState.Get());

		PlayerInfo.Text = $"{player?.Name}  {TranslationServer.Translate("INN_OWNER")}  {player?.Inn?.Name}";
	}

	private void GoToAdventure()
	{
		GetTree().ChangeScene("res://scenes/AdventureScene.tscn");
	}

	private void GoToInn()
	{
		GetTree().ChangeScene("res://scenes/InnScene.tscn");
	}

	private void GoToWorld()
	{
		GetTree().ChangeScene("res://scenes/WorldScene.tscn");
	}
}
