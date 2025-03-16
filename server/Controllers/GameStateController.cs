using HelteOgHulerServer.Logic;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GameStateController(GameStateLogic gameStateLogic) : ControllerBase
{
    [HttpGet(Name = "GetGameState")]
    public ActionResult<string> Get()
    {
        User user = (User)HttpContext.Items["User"]!;

        GameState gameState = user.IsAdmin
            ? gameStateLogic.Get()
            : gameStateLogic.Get(user.PlayerId);

        return HHJsonSerializer.Serialize(gameState);
    }
}
