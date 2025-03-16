using HelteOgHulerServer.Logic;
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

        if (user.IsAdmin == true)
        {
            return HHJsonSerializer.Serialize(gameStateLogic.Get());
        }

        return HHJsonSerializer.Serialize(gameStateLogic.Get(user.PlayerId));
    }
}
