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
    public string[] Headers = ["HHPlayerName: "];
    public HTTPRequest Request { get; set; }
    public ResponseWrapper Response { get; set; }

    public RequestNode(Node parent, ResponseType type)
    {
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
}
