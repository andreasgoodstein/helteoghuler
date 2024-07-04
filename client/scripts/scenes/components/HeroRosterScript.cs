using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using System.Collections.Generic;
using System;

namespace HelteOgHulerClient;

public class HeroRosterScript : VBoxContainer, ISubscriber<GameState>
{
    private Dictionary<Guid, Node> HeroRosterItemDict = [];

    private readonly PackedScene HeroRosterItemScene = GD.Load<PackedScene>("res://scenes/components/HeroRosterItem.tscn");

    public override void _Ready()
    {
        Message(GlobalGameState.Get());

        GlobalGameState.Register(this);
    }

    public override void _ExitTree()
    {
        GlobalGameState.Unregister(this);
    }

    public void Message(GameState gameState)
    {
        var heroRoster = GameStateHelper.GetPlayer(gameState)?.Inn?.HeroRoster;

        // Add Heros to HeroRoster container
        foreach (var hero in heroRoster.Values)
        {
            if (!HeroRosterItemDict.ContainsKey(hero.Id))
            {
                var item = HeroRosterItemScene.Instance();
                item.GetNode<Label>("HeroName").Text = hero.Name;
                item.GetNode<Label>("HeroStatus").Text = "Ready";

                AddChild(item);
                HeroRosterItemDict.Add(hero.Id, item);
            }
        }

        // Remove Heros from HeroRoster container
        foreach (var heroId in HeroRosterItemDict.Keys)
        {
            if (!heroRoster.ContainsKey(heroId.ToString()))
            {
                RemoveChild(HeroRosterItemDict[heroId]);
                HeroRosterItemDict.Remove(heroId);
            }
        }
    }

    public string GetId()
    {
        return Filename + Name;
    }
}
