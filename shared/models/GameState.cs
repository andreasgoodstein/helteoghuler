namespace HelteOgHulerShared.Models
{

    public class GameState
    {
        public WorldState World { get; set; } = new WorldState { };

        public Player Player { get; set; }
    }

    public class WorldState
    {
        public string WorldName { get; set; } = "default";

        public ulong TotalAdventures { get; set; }
    }
}
