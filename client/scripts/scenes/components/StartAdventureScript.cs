using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using System;

public class StartAdventureScript : Button, ISubscriber<GameState>
{
	private DateTime? RestUntil;

	private Button StartAdventure;

	private Timer WaitUntilRestedTimer;

	private string StartAdventureDefaultText;

	private readonly string StartAdventureDisabledText = "Party must rest for another: ";

	public override void _Ready()
	{
		StartAdventure = GetNode<Button>("../StartAdventureButton");

		StartAdventureDefaultText = StartAdventure.Text;

		StartAdventure.Connect("pressed", this, "StartAdventurePressed");

		Message(GlobalGameState.Get());

		GlobalGameState.Register(this);
	}

	public override void _ExitTree()
	{
		GlobalGameState.Unregister(this);
	}

	private async void StartAdventurePressed()
	{
		StartAdventure.Disabled = true;

		var adventure = await GetNode<Server>("/root/Server").StartAdventure(this);

		GetNode<Label>("%Message").Text = GetAdventureText(adventure);
	}

	private string GetAdventureText(Adventure adventure)
	{
		var result = $"The adventure was {adventure.Status}!\n";

		result += $"The Party returned with {adventure.Gold} gold.\n\n";

		foreach (var encounter in adventure.EncounterList)
		{
			result += $"Your Party encountered {encounter.Monster.Name}\n\n";

			foreach (var action in encounter.ActionLog)
			{
				result += $"{action}\n";
			}
		}

		return result;
	}

	private void SetupRestedTimer(Player player)
	{
		if (player?.RestUntil == null)
		{
			return;
		}

		RestUntil = player.RestUntil;

		WaitUntilRested();

		WaitUntilRestedTimer = new Timer
		{
			Autostart = true,
		};

		WaitUntilRestedTimer.Connect("timeout", this, "WaitUntilRested");

		AddChild(WaitUntilRestedTimer);
	}

	private void WaitUntilRested()
	{
		if (RestUntil != null && RestUntil > DateTime.UtcNow)
		{
			var timeSpan = RestUntil.Value - DateTime.UtcNow;
			StartAdventure.Disabled = true;
			StartAdventure.Text = $"{StartAdventureDisabledText} {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
			return;
		}

		RestUntil = null;

		WaitUntilRestedTimer?.Stop();
		WaitUntilRestedTimer?.QueueFree();
		WaitUntilRestedTimer = null;

		StartAdventure.Disabled = false;
		StartAdventure.Text = StartAdventureDefaultText;
	}

	public string GetId()
	{
		return Filename + Name;
	}

	public void Message(GameState message)
	{
		var player = GameStateHelper.GetPlayer(message);

		if ((player?.Inn?.HeroRoster?.Count ?? 0) < 1)
		{
			StartAdventure.Disabled = true;
			StartAdventure.Text = "NO_PARTY_TO_VENTURE_FORTH";
			return;
		}

		StartAdventure.Disabled = false;
		StartAdventure.Text = StartAdventureDefaultText;

		SetupRestedTimer(player);
	}
}
