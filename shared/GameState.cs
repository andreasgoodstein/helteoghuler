namespace HelteOgHulerShared
{

    public class GameState
    {
        public WorldState World { get; set; } = new WorldState { };
    }

    public class WorldState
    {
        public string WorldName { get; set; } = "default";
    }
}
