using Godot;
using HelteOgHulerClient.Interfaces;
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

	private void StartAdventurePressed()
	{
		GetNode<Server>("/root/Server").StartAdventure(this);
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
		var player = message?.PrivatePlayerDict?.Values?.GetEnumerator().Current;

		if (player?.RestUntil != null)
		{
			RestUntil = player.RestUntil;

			WaitUntilRested();

			WaitUntilRestedTimer = new Timer
			{
				Autostart = true,
			};

			WaitUntilRestedTimer.Connect("timeout", this, "WaitUntilRested");

			AddChild(WaitUntilRestedTimer);
		}
	}
}
