using HelteOgHulerServer.Events;
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

    public PlayerController(
        EventService eventService,
        GameStateLogic gameStateLogic,
        PlayerLogic playerLogic
    )
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
            var newPlayer = _playerLogic.CreatePlayer(
                _gameStateLogic.Get(),
                user.PlayerId,
                innName,
                playerName
            );
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
                StatusCode = 400,
            };
        }
    }

    [HttpGet(Name = "CompleteObjective")]
    public async Task<ActionResult<string>> CompleteObjective(PlayerObjective objective)
    {
        User user = (User)HttpContext.Items["User"]!;

        try
        {
            _playerLogic.CanCompleteObjective(_gameStateLogic.Get(), user.PlayerId, objective);

            var completeObjectiveEvent = new CompleteObjectiveEvent_V1
            {
                CreatedAt = DateTime.UtcNow,
                Objective = objective,
                PlayerId = user.PlayerId,
            };

            await _eventService.CreateAsync(completeObjectiveEvent);

            _gameStateLogic.UpdateGameState(completeObjectiveEvent);

            return HHJsonSerializer.Serialize(completeObjectiveEvent);
        }
        catch (InvalidDataException exception)
        {
            return new ContentResult
            {
                Content = HHJsonSerializer.Serialize(new HHError { Message = exception.Message }),
                StatusCode = 400,
            };
        }
    }
}
