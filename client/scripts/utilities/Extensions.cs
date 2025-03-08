using Godot;
using HelteOgHulerClient;

public static class Extensions
{
    public static Server GetServer(this Node node)
    {
        return node.GetNode<Server>("/root/Server");
    }
}
