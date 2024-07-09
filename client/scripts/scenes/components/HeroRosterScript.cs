using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using System.Collections.Generic;

namespace HelteOgHulerClient;

public class HeroRosterScript : Control, ISubscriber<GameState>
{
	private readonly PackedScene HeroRosterItem = GD.Load<PackedScene>("res://scenes/components/HeroRosterItem.tscn");
	private VBoxContainer HeroList;

	public override void _Ready()
	{
		HeroList = GetNode<VBoxContainer>("%HeroList");

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
		var heroRoster = GameStateHelper.GetPlayer(gameState)?.Inn?.HeroRoster?.Values ?? new Dictionary<string, Hero>().Values;
		var isResting = GameStateHelper.IsResting(gameState);

		foreach (Node child in HeroList?.GetChildren())
		{
			HeroList.RemoveChild(child);
		}

		foreach (var hero in heroRoster)
		{
			var item = HeroRosterItem.Instance();

			item.GetNode<Label>("HeroName").Text = hero.Name;
			item.GetNode<Label>("HeroStatus").Text = isResting ? "Resting" : "Ready";

			HeroList.AddChild(item);
		}
	}
}
