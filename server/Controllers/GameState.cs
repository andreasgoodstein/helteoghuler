using Microsoft.AspNetCore.Mvc;
using HelteOgHulerShared;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GameStateController : ControllerBase
{
    [HttpGet(Name = "GetGameState")]
    public GameState Get()
    {
        return new GameState { World = new WorldState { WorldName = "Konia" } };
    }
}