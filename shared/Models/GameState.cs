namespace HelteOgHulerShared.Models;

public class GameState
{
    public DateTime? CurrentTime { get; set; }

    public string ErrorMessage { get; set; }

    public Dictionary<Guid, Player> PrivatePlayerDict { get; set; }

    public Dictionary<Guid, PlayerPublic> PublicPlayerDict { get; set; }

    public World World { get; set; }
}

public class World
{
    public string Name { get; set; }

    public ulong TotalAdventures { get; set; }
}
