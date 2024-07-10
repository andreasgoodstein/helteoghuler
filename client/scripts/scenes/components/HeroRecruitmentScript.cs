using Godot;
using HelteOgHulerClient;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using System;
using System.Collections.Generic;

public class HeroRecruitmentScript : Control, ISubscriber<GameState>
{
	private readonly PackedScene HeroRecruitmentItem = GD.Load<PackedScene>("res://scenes/components/HeroRecruitmentItem.tscn");

	private VBoxContainer RecruitList;

	public override void _Ready()
	{
		RecruitList = GetNode<VBoxContainer>("%RecruitList");

		Message(GlobalGameState.Get());

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

	public void Message(GameState gameState)
	{
		var recruitList = GameStateHelper.GetPlayer(gameState)?.Inn?.HeroRecruits?.Values ?? new Dictionary<string, Hero>().Values;

		foreach (Node child in RecruitList?.GetChildren())
		{
			RecruitList.RemoveChild(child);
		}

		foreach (var hero in recruitList)
		{
			var item = HeroRecruitmentItem.Instance();

			item.GetNode<Label>("%RecruitName").Text = hero.Name;
			item.GetNode<Label>("%RecruitClass").Text = TranslationServer.Translate("HERO");
			item.GetNode<Label>("%RecruitPrice").Text = 200.ToString();

			item.GetNode<Button>("%ButtonRecruit").Connect("pressed", this, "RecruitHero", [hero.Id.ToString()]);

			RecruitList.AddChild(item);
		}
	}

	private void RecruitHero(string heroId)
	{
		GD.Print("Recruiting " + heroId);
		// TODO: Implement client side RecruitHero request
	}
}
