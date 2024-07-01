using System;
using Godot;
using HelteOgHulerClient.Services;

namespace HelteOgHulerClient.Utilities;

public enum ResponseType
{
    JSONCallback = 0,
    StringCallback = 1,
}

public class RequestNode
{
    private Node _parent;
    public string[] Headers = ["HHLoginName: "];
    private HTTPRequest Request { get; set; }
    private ResponseWrapper Response { get; set; }

    public RequestNode(Node parent, ResponseType type)
    {
        Headers[0] = Headers[0] + ClientStorage.GetLoginName();

        Request = new HTTPRequest();
        Response = new ResponseWrapper();

        var callbackName = type == ResponseType.JSONCallback ? "JSONCallback" : "StringCallback";

        Request.Connect("request_completed", Response, callbackName);

        _parent = parent;
        _parent.AddChild(Request);
        Request.AddChild(Response);
    }

    public void Clean()
    {
        Request?.CancelRequest();
        Request?.RemoveChild(Response);
        _parent?.RemoveChild(Request);

        Request = null;
        Response = null;
        _parent = null;
    }

    public void ExecuteRequest(string url)
    {
        Request.Request(url, Headers);
    }

    public void SetResponseHandler(Action<int, int, string[], byte[]> handler)
    {
        Response.JSONCallbackDelegate = handler;
    }
    public void SetResponseHandler(Action<int, int, string[], string> handler)
    {
        Response.TextCallbackDelegate = handler;
    }
}
