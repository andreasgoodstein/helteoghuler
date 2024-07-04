using HelteOgHulerServer.Logic;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PlayerController : ControllerBase
{
    private readonly string ERROR_400 = HHJsonSerializer.Serialize(new HHError { Message = "This deed has already been claimed" });

    private readonly GameStateLogic _gameStateLogic;

    private readonly PlayerLogic _playerLogic;

    public PlayerController(GameStateLogic gameStateLogic, PlayerLogic playerLogic)
    {
        _gameStateLogic = gameStateLogic;
        _playerLogic = playerLogic;
    }

    [HttpGet(Name = "New")]
    public async Task<ActionResult<string>> New(string innName, string playerName)
    {
        User user = (User)HttpContext.Items["User"]!;

        // Move command check to logic layer (PlayerLogic.cs)
        if (_gameStateLogic.Get().PrivatePlayerDict.ContainsKey(user.PlayerId))
        {
            return new ContentResult
            {
                Content = ERROR_400,
                StatusCode = 400,
            };
        }

        await _playerLogic.CreatePlayer(user.PlayerId, innName, playerName);

        return HHJsonSerializer.Serialize(_gameStateLogic.Get(user.PlayerId));
    }
}
