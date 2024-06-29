using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PlayerController : ControllerBase
{
    private readonly GameStateLogic _gameStateLogic;

    private readonly PlayerLogic _playerLogic;

    public PlayerController(GameStateLogic gameStateLogic, PlayerLogic playerLogic)
    {
        _gameStateLogic = gameStateLogic;
        _playerLogic = playerLogic;
    }

    [HttpGet(Name = "NewPlayer")]
    public async Task<ActionResult<string>> NewPlayer(string playerName, string innName)
    {
        User user = (User)HttpContext.Items["User"]!;

        await _playerLogic.CreatePlayer(user.PlayerId, playerName, innName);

        return HHJsonSerializer.Serialize(_gameStateLogic.Get(user.PlayerId));
    }
}
