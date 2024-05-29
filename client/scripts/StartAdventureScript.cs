using Godot;
using System;

public class StartAdventureScript : Button
{
    private HTTPRequest request;

    public override void _Ready()
    {

        Button button = GetNode<Button>("../StartAdventureButton");
        request = GetNode<HTTPRequest>("HttpStartAdventure");

        request.Connect("request_completed", this, "OnRequestCompleted");
        button.Connect("pressed", this, "StartAdventurePressed");
    }

    private void StartAdventurePressed()
    {
        request.Request("https://localhost:7111/Adventure/Start");
    }

    private void OnRequestCompleted(int result, int response_code, string[] headers, byte[] body)
    {
        if (response_code < 200 || response_code > 299)
        {
            GD.Print("Could not go on adventure");
            return;
        }

        GD.Print("Adventure Completed");
    }
}