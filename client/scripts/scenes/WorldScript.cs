using Godot;
using HelteOgHulerClient.Interfaces;
using HelteOgHulerClient;
using HelteOgHulerShared.Models;

public class WorldScript : Control, ISubscriber<GameState>
{
    private KeyValueLabel TotalAdventures;

    public override void _Ready()
    {
        GlobalGameState.Register(this);

        GameState gameState = GlobalGameState.Get();

        TotalAdventures = GetNode<KeyValueLabel>("TotalAdventuresTracker");
        TotalAdventures.Set(gameState?.World?.TotalAdventures ?? 0);
    }

    public override void _ExitTree()
    {
        GlobalGameState.Unregister(this);
    }

    public void Message(GameState gameState)
    {
        if (gameState?.World?.TotalAdventures != null)
        {
            TotalAdventures.Set(gameState.World.TotalAdventures);
        }
    }

    public string GetId()
    {
        return Filename + Name;
    }
}
