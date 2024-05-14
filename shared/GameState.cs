namespace HelteOgHulerShared;

public class GameState
{
    public WorldState World { get; init; } = new WorldState { };
}

public class WorldState
{
    public string WorldName { get; init; } = "default";
}
