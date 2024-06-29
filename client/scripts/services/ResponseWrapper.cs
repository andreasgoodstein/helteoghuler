using Godot;
using System;

namespace HelteOgHulerClient.Services;

public class ResponseWrapper : Node
{
    public void JSONCallback(int result, int response_code, string[] headers, byte[] body)
    {
        JSONCallbackDelegate.Invoke(result, response_code, headers, body);
    }

    public void TextCallback(int result, int response_code, string[] headers, string body)
    {
        TextCallbackDelegate.Invoke(result, response_code, headers, body);
    }

    public Action<int, int, string[], byte[]> JSONCallbackDelegate { get; set; }
    public Action<int, int, string[], string> TextCallbackDelegate { get; set; }
}
