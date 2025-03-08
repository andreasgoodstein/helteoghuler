using System;
using Godot;
using HelteOgHulerShared.Models;

namespace HelteOgHulerClient.Services;

public class ResponseWrapper() : Node
{
    public Action ErrorDelegate { get; set; }
    public Action<byte[]> JSONCallbackDelegate { get; set; }
    public Action<string> TextCallbackDelegate { get; set; }
    private Action Clean;

    private void HandleUnauthorizedError()
    {
        GD.PrintErr("Auth: Unauthorized");
        GetTree().ChangeScene("res://scenes/LoginMenuScene.tscn");
    }

    // Cannot set with constructor because of Godot
    public void SetCleaner(Action clean)
    {
        Clean = clean;
    }

    public void JSONCallback(int result, int response_code, string[] headers, byte[] body)
    {
        // Unauthorized
        if (response_code == 401)
        {
            HandleUnauthorizedError();
            ErrorDelegate?.Invoke();
        }
        // Bad Request
        else if (response_code == 400)
        {
            ResponseHandler.HandleGameStateResponse<HHError>(body);
            ErrorDelegate?.Invoke();
        }
        // Unexpected Errors
        else if (response_code < 200 || response_code > 299)
        {
            ErrorDelegate?.Invoke();
        }
        // Success
        else
        {
            JSONCallbackDelegate?.Invoke(body);
        }

        Clean();
    }

    public void TextCallback(int result, int response_code, string[] headers, string body)
    {
        // Unauthorized
        if (response_code == 401)
        {
            HandleUnauthorizedError();
            ErrorDelegate?.Invoke();
        }
        // Unexpected Errors
        else if (response_code < 200 || response_code > 299)
        {
            ErrorDelegate?.Invoke();
        }
        // Success
        else
        {
            TextCallbackDelegate?.Invoke(body);
        }

        Clean();
    }
}
