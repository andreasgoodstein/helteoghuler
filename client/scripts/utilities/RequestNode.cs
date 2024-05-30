using Godot;

namespace HelteOgHulerClient.Utilities;

public class RequestNode
{
    private Node _parent;
    public HTTPRequest Request { get; set; }

    public RequestNode(Node parent)
    {
        Request = new HTTPRequest();

        _parent = parent;
        _parent.AddChild(Request);
    }

    public void Clean()
    {
        Request.CancelRequest();
        _parent.RemoveChild(Request);

        Request = null;
        _parent = null;
    }
}
