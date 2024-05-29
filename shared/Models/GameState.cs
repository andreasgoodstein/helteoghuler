namespace HelteOgHulerShared.Models;

public class GameState
{
    public WorldState World { get; set; }

    public Player Player { get; set; }
}

public class WorldState
{
    public string WorldName { get; set; }

    public ulong TotalAdventures { get; set; }
}
