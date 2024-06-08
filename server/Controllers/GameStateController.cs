using HelteOgHulerServer.Logic;
using HelteOgHulerShared.Models;
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
        return HHJsonSerializer.Serialize(_gameStateLogic.Get());
    }
}