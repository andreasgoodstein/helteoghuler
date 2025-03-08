using Godot;
using HelteOgHulerClient;

public static class Extensions
{
    public static Server GetServer(this Node node)
    {
        return node.GetNode<Server>("/root/Server");
    }

    public static Settings GetSettings(this Node node)
    {
        return node.GetNode<Settings>("/root/Settings");
    }
}
