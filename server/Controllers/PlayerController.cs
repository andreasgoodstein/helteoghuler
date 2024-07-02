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

    [HttpGet(Name = "New")]
    public async Task<ActionResult<string>> New(string playerName, string innName)
    {
        User user = (User)HttpContext.Items["User"]!;

        // Check for existing player
        if (_gameStateLogic.Get().PrivatePlayerDict.ContainsKey(user.PlayerId))
        {
            return new ContentResult
            {
                Content = "This deed has already been claimed",
                StatusCode = 400,
            };
        }

        await _playerLogic.CreatePlayer(user.PlayerId, playerName, innName);

        return HHJsonSerializer.Serialize(_gameStateLogic.Get(user.PlayerId));
    }
}
