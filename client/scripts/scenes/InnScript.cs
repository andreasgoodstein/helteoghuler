using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

public class InnScript : Control, ISubscriber<GameState>
{
	private Control HeroRecruitment;
	private Control HeroRoster;
	private Control UpgradeInn;

	private Button RecruitHeroButton;
	private Button ViewHeroButton;
	private Button UpgradeInnButton;

	public override void _Ready()
	{
		RecruitHeroButton = GetNode<Button>("%ButtonRecruitHeroes");
		RecruitHeroButton.Connect("pressed", this, "ButtonRecruitHeroesOnClick");

		ViewHeroButton = GetNode<Button>("%ButtonViewHeroes");
		ViewHeroButton.Connect("pressed", this, "ButtonViewHeroesOnClick");

		UpgradeInnButton = GetNode<Button>("%ButtonUpgradeInn");
		UpgradeInnButton.Connect("pressed", this, "ButtonUpgradeInnOnClick");

		HeroRecruitment = GetNode<Control>("%HeroRecruitment");
		HeroRoster = GetNode<Control>("%HeroRoster");
		UpgradeInn = GetNode<Control>("%UpgradeInn");

		Message(GlobalGameState.Get());

		GlobalGameState.Register(this);
	}

	private void ButtonRecruitHeroesOnClick()
	{
		HeroRoster.SetProcess(false);
		HeroRoster.Hide();

		UpgradeInn.SetProcess(false);
		UpgradeInn.Hide();

		HeroRecruitment.Show();
		HeroRecruitment.SetProcess(true);
	}

	private void ButtonViewHeroesOnClick()
	{
		HeroRecruitment.SetProcess(false);
		HeroRecruitment.Hide();

		UpgradeInn.SetProcess(false);
		UpgradeInn.Hide();

		HeroRoster.Show();
		HeroRoster.SetProcess(true);
	}

	private void ButtonUpgradeInnOnClick()
	{
		HeroRecruitment.SetProcess(false);
		HeroRecruitment.Hide();

		HeroRoster.SetProcess(false);
		HeroRoster.Hide();

		UpgradeInn.Show();
		UpgradeInn.SetProcess(true);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	public void Message(GameState gameState)
	{
		var player = gameState.GetPlayer();

		GetNode<Label>("%Gold").Text = player?.Inn?.Chest?.Gold.ToString() ?? "0";

		RecruitHeroButton.Disabled = false;
		ViewHeroButton.Disabled = false;
		UpgradeInnButton.Disabled = false;

		if (player?.Inn?.HeroRecruits?.Count < 1)
		{
			RecruitHeroButton.Disabled = true;
		}

		if (player?.Inn?.HeroRoster?.Count < 1)
		{
			ViewHeroButton.Disabled = true;
		}

		if (player?.Inn?.BuiltUpgrades?.Contains(InnUpgrade.DiscoverWorkshop) == false)
		{
			UpgradeInnButton.Disabled = true;
		}
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
