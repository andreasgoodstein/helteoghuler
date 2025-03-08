using System;
using Godot;
using HelteOgHulerClient;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerShared.Models;

public class LoginMenuScript : Control, ISubscriber<GameState>
{
    private Button StartAdventure;
    private LineEdit UserNameInput;
    private LoadingSpinnerScript LoadingSpinner;

    public override void _Ready()
    {
        LoadingSpinner = GetNode<LoadingSpinnerScript>("%Loading");
        StartAdventure = GetNode<Button>("%StartAdventure");
        UserNameInput = GetNode<LineEdit>("%StartAdventure/UserNameInput");

        StartAdventure.Connect("pressed", this, "StartAdventurePressed");

        GlobalGameState.Register(this);

        var loginName = this.GetSettings().LoginName;

        if (!String.IsNullOrWhiteSpace(loginName))
        {
            UserNameInput.Text = loginName;
        }
    }

    public override void _ExitTree()
    {
        GlobalGameState.Unregister(this);
    }

    public string GetId()
    {
        return Filename + Name;
    }

    private void StartAdventurePressed()
    {
        if (String.IsNullOrWhiteSpace(UserNameInput.Text))
        {
            return;
        }

        this.GetSettings().LoginName = UserNameInput.Text;

        this.GetServer().RefreshGameState(this);

        StartAdventure.Hide();
        LoadingSpinner.Show();
    }

    public void Message(GameState message)
    {
        if (message?.PrivatePlayerDict?.Count < 1)
        {
            GetTree().ChangeScene("res://scenes/NewPlayerScene.tscn");
        }
        else
        {
            GetTree().ChangeScene("res://scenes/InnScene.tscn");
        }
    }
}
