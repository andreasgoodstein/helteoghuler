namespace HelteOgHulerShared.Models;

public class GameState
{
    public World World { get; set; }

    public Player Player { get; set; }
}

public class World
{
    public string Name { get; set; }

    public ulong TotalAdventures { get; set; }
}
