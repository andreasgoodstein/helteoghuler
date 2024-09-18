using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PlayerController : ControllerBase
{
    private readonly EventService _eventService;
    private readonly GameStateLogic _gameStateLogic;
    private readonly PlayerLogic _playerLogic;

    public PlayerController(EventService eventService, GameStateLogic gameStateLogic, PlayerLogic playerLogic)
    {
        _eventService = eventService;
        _gameStateLogic = gameStateLogic;
        _playerLogic = playerLogic;
    }

    [HttpGet(Name = "New")]
    public async Task<ActionResult<string>> New(string innName, string playerName)
    {
        User user = (User)HttpContext.Items["User"]!;

        try
        {
            var newPlayer = _playerLogic.CreatePlayer(_gameStateLogic.Get(), user.PlayerId, innName, playerName);
            var newPlayerEvent = new NewPlayerEvent_V1
            {
                CreatedAt = DateTime.UtcNow,
                Player = newPlayer,
            };

            await _eventService.CreateAsync(newPlayerEvent);

            _gameStateLogic.UpdateGameState(newPlayerEvent);

            return HHJsonSerializer.Serialize(_gameStateLogic.Get(user.PlayerId));
        }
        catch (InvalidDataException exception)
        {
            return new ContentResult
            {
                Content = HHJsonSerializer.Serialize(new HHError { Message = exception.Message }),
                StatusCode = 500,
            };
        }
    }
}
