using System;
using Godot;
using HelteOgHulerClient;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

public class UpgradeInnScript : Control, ISubscriber<GameState>
{
    private readonly PackedScene InnUpgradeItemScene = GD.Load<PackedScene>(
        "res://scenes/components/UpgradeInnItem.tscn"
    );
    private VBoxContainer UpgradeItemList;

    public override void _Ready()
    {
        UpgradeItemList = GetNode<VBoxContainer>("%UpgradeItemList");

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
        var player = message.GetPlayer();
        var inn = player?.Inn;

        PopulatePendingItem(inn);
        PopulateUpgradeList(inn);
        ShowPopupOnProgress(player);
    }

    private void PopulatePendingItem(Inn inn)
    {
        var pendingSection = GetNode<Control>("%PendingUpgrade");

        var pendingUpgrade = inn?.PendingUpgrade;
        if (pendingUpgrade == null)
        {
            pendingSection.Hide();
            return;
        }

        pendingSection.Show();

        var name = pendingSection.GetNode<Label>("%UpgradeName");

        if (name != null)
        {
            name.Text = Enum.GetName(typeof(InnUpgradeName), pendingUpgrade);
        }
    }

    private void PopulateUpgradeList(Inn inn)
    {
        var availableUpgrades = inn?.AvailableUpgrades;
        var pendingUpgrade = inn?.PendingUpgrade;
        var playerGold = inn?.Chest?.Gold ?? 0;

        foreach (Node child in UpgradeItemList?.GetChildren())
        {
            UpgradeItemList.RemoveChild(child);
        }

        foreach (var upgrade in availableUpgrades)
        {
            var item = InnUpgradeItemScene.Instance();
            var upgradeButton = item.GetNode<Button>("%UpgradeButton");
            var upgradeCost = InnUpgrades.Cost[upgrade];

            item.GetNode<Label>("%UpgradeName").Text = Enum.GetName(
                typeof(InnUpgradeName),
                upgrade
            );
            item.GetNode<Label>("%UpgradeCost").Text = upgradeCost.ToString();

            upgradeButton.Connect("pressed", this, "BuildUpgrade", [upgrade]);

            if (pendingUpgrade != null || playerGold < upgradeCost)
            {
                upgradeButton.Disabled = true;
            }

            UpgradeItemList.AddChild(item);
        }
    }

    private void ShowPopupOnProgress(Player player)
    {
        if (player.ObjectivesCompleted?.ContainsKey(PlayerObjectives.DiscoverWorkshop) != true)
        {
            var popup = GetNode<Popup>("%UpgradeInnPopup");
            popup.GetNode<Label>("Scroll/Text").Text = "UPGRADE_INN_INTRO";
            popup.Show();
            popup.Connect("confirmed", this, "ConfirmPopup", [PlayerObjectives.DiscoverWorkshop]);
        }
    }

    private void ConfirmPopup(PlayerObjectives objective)
    {
        var popup = GetNode<Popup>("%UpgradeInnPopup");

        switch (objective)
        {
            case PlayerObjectives.DiscoverWorkshop:
                // TODO: Register objective completed
                break;
        }

        popup.Hide();
    }

    private async void BuildUpgrade(InnUpgradeName upgrade)
    {
        await GetNode<Server>("/root/Server").UpgradeInn(this, upgrade);
    }
}
