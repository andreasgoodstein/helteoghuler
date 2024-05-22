using Microsoft.AspNetCore.Mvc;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GameStateController : ControllerBase
{
    [HttpGet(Name = "GetGameState")]
    public string Get()
    {
        return HHJsonSerializer.Serialize(new GameState { World = new WorldState { WorldName = "Konia" }, Player = new Player { Name = "TestPlayer" } });
    }
}