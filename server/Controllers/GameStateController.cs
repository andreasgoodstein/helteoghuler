using HelteOgHulerServer.Logic;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GameStateController : ControllerBase
{
    private readonly GameStateLogic _gameStateLogic;

    public GameStateController(GameStateLogic gameStateLogic)
    {
        _gameStateLogic = gameStateLogic;
    }

    [HttpGet(Name = "GetGameState")]
    public string Get()
    {
        User user = (User)HttpContext.Items["User"]!;

        if (user.IsAdmin == true)
        {
            return HHJsonSerializer.Serialize(_gameStateLogic.Get());
        }

        return HHJsonSerializer.Serialize(_gameStateLogic.Get(user.PlayerId));
    }
}