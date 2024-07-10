using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;
using System.Collections.Generic;
using System;
using System.Linq;

public class PublicInnkeeperScoreScript : Control, ISubscriber<GameState>
{
	private readonly PackedScene PublicInnkeeperScoreItem = GD.Load<PackedScene>("res://scenes/components/PublicInnkeeperScoreItem.tscn");

	private VBoxContainer InnkeeperList;

	public override void _Ready()
	{
		InnkeeperList = GetNode<VBoxContainer>("%InnkeeperList");

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

	public void Message(GameState message)
	{
		var publicPlayerCollection = GlobalGameState.Get()?.PublicPlayerDict?.Values ?? new Dictionary<Guid, PlayerPublic>().Values;
		var publicPlayerList = publicPlayerCollection.ToList()
			.OrderBy(player => player.Name)
			.OrderByDescending(player => player.TotalGoldEarned);

		var index = 0;
		foreach (Node child in InnkeeperList.GetChildren())
		{
			if (index == 0)
			{
				continue;
			}

			InnkeeperList.RemoveChild(child);
			index += 1;
		}

		foreach (PlayerPublic publicPlayer in publicPlayerList)
		{
			Node item = PublicInnkeeperScoreItem.Instance();

			item.GetNode<Label>("Container/Innkeeper").Text = publicPlayer.Name;
			item.GetNode<Label>("Container/Inn").Text = publicPlayer.InnName;
			item.GetNode<Label>("Container/CareerGold").Text = publicPlayer.TotalGoldEarned.ToString();

			InnkeeperList.AddChild(item);
		}
	}
}
