using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class InnScript : Control, ISubscriber<GameState>
{
	private Control HeroRecruitment;
	private Control HeroRoster;

	private Button RecruitHeroButton;
	private Button ViewHeroButton;

	public override void _Ready()
	{
		RecruitHeroButton = GetNode<Button>("%ButtonRecruitHeroes");
		RecruitHeroButton.Connect("pressed", this, "ButtonRecruitHeroesOnClick");

		ViewHeroButton = GetNode<Button>("%ButtonViewHeroes");
		ViewHeroButton.Connect("pressed", this, "ButtonViewHeroesOnClick");

		HeroRecruitment = GetNode<Control>("%HeroRecruitment");
		HeroRoster = GetNode<Control>("%HeroRoster");

		Message(GlobalGameState.Get());

		GlobalGameState.Register(this);
	}

	private void ButtonRecruitHeroesOnClick()
	{
		HeroRoster.SetProcess(false);
		HeroRoster.Hide();

		HeroRecruitment.Show();
		HeroRecruitment.SetProcess(true);
	}

	private void ButtonViewHeroesOnClick()
	{
		HeroRecruitment.SetProcess(false);
		HeroRecruitment.Hide();

		HeroRoster.Show();
		HeroRoster.SetProcess(true);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		var player = GameStateHelper.GetPlayer(gameState);

		GetNode<Label>("%Gold").Text = player?.Inn?.Chest?.Gold.ToString() ?? "0";

		RecruitHeroButton.Disabled = false;
		ViewHeroButton.Disabled = false;

		if (player?.Inn?.HeroRecruits?.Count < 1)
		{
			RecruitHeroButton.Disabled = true;
		}

		if (player?.Inn?.HeroRoster?.Count < 1)
		{
			ViewHeroButton.Disabled = true;
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
