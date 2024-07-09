using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class InnScript : Control//, ISubscriber<GameState>
{
	private Control HeroRecruitment;
	private Control HeroRoster;

	public override void _Ready()
	{
		GetNode<Button>("%ButtonRecruitHeroes").Connect("pressed", this, "ButtonRecruitHeroesOnClick");
		GetNode<Button>("%ButtonViewHeroes").Connect("pressed", this, "ButtonViewHeroesOnClick");

		HeroRecruitment = GetNode<Control>("%HeroRecruitment");
		HeroRoster = GetNode<Control>("%HeroRoster");

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

	// Message(GlobalGameState.Get());

	// GlobalGameState.Register(this);
	// }

	// public override void _ExitTree()
	// {
	// 	GlobalGameState.Unregister(this);
	// }

	// public void Message(GameState gameState)
	// {
	// }

	// public string GetId()
	// {
	// 	return Filename + Name;
	// }
}

//TODO: Implement Hero recruitment interface
// Two lists with the hero recruits and hero roster
// Select recruit and click button to trigger recruitment
