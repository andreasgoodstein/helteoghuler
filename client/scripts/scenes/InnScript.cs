using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class InnScript : Control, ISubscriber<GameState>
{
	private Control HeroRecruitment;
	private Control HeroRoster;

	public override void _Ready()
	{
		GetNode<Button>("%ButtonRecruitHeroes").Connect("pressed", this, "ButtonRecruitHeroesOnClick");
		GetNode<Button>("%ButtonViewHeroes").Connect("pressed", this, "ButtonViewHeroesOnClick");

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
		GetNode<Label>("%Gold").Text = GameStateHelper.GetPlayer(gameState)?.Inn?.Chest?.Gold.ToString() ?? "0";
	}

	public string GetId()
	{
		return Filename + Name;
	}
}
