using Godot;
using HelteOgHulerShared.Models;
using System;

namespace HelteOgHulerClient.Services;

public class ResponseWrapper : Node
{
    public Action ErrorDelegate { get; set; }
    public Action<byte[]> JSONCallbackDelegate { get; set; }
    public Action<string> TextCallbackDelegate { get; set; }

    private void HandleUnauthorizedError()
    {
        GD.PrintErr("Auth: Unauthorized");
        GetTree().ChangeScene("res://scenes/LoginMenuScene.tscn");
    }

    public void JSONCallback(int result, int response_code, string[] headers, byte[] body)
    {
        // Unauthorized
        if (response_code == 401)
        {
            HandleUnauthorizedError();
            ErrorDelegate.Invoke();
        }
        // Bad Request
        else if (response_code == 400)
        {
            ResponseHandler.HandleGameStateResponse<HHError>(body);
            ErrorDelegate.Invoke();
        }
        // Unexpected Errors
        else if (response_code < 200 || response_code > 299)
        {
            ErrorDelegate.Invoke();
        }
        // Success
        else
        {
            JSONCallbackDelegate.Invoke(body);
        }
    }

    public void TextCallback(int result, int response_code, string[] headers, string body)
    {
        // Unauthorized
        if (response_code == 401)
        {
            HandleUnauthorizedError();
            ErrorDelegate.Invoke();
        }
        // Unexpected Errors
        else if (response_code < 200 || response_code > 299)
        {
            ErrorDelegate.Invoke();
        }
        // Success
        else
        {
            TextCallbackDelegate.Invoke(body);
        }
    }

}
