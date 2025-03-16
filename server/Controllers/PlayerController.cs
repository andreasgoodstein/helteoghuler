using HelteOgHulerServer.Events;
using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PlayerController(
    EventService eventService,
    GameStateLogic gameStateLogic,
    PlayerLogic playerLogic
) : ControllerBase
{
    [HttpGet(Name = "New")]
    public async Task<ActionResult<string>> New(string innName, string playerName)
    {
        User user = (User)HttpContext.Items["User"]!;

        try
        {
            var newPlayer = playerLogic.CreatePlayer(
                gameStateLogic.Get(),
                user.PlayerId,
                innName,
                playerName
            );
            var newPlayerEvent = new NewPlayerEvent_V1
            {
                CreatedAt = DateTime.UtcNow,
                Player = newPlayer,
            };

            await eventService.CreateAsync(newPlayerEvent);

            gameStateLogic.UpdateGameState(newPlayerEvent);

            return HHJsonSerializer.Serialize(gameStateLogic.Get(user.PlayerId));
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
            playerLogic.CanCompleteObjective(gameStateLogic.Get(), user.PlayerId, objective);

            var completeObjectiveEvent = new CompleteObjectiveEvent_V1
            {
                CreatedAt = DateTime.UtcNow,
                CompleteObjective = new() { Objective = objective, PlayerId = user.PlayerId },
            };

            await eventService.CreateAsync(completeObjectiveEvent);

            gameStateLogic.UpdateGameState(completeObjectiveEvent);

            return HHJsonSerializer.Serialize<CompleteObjective>(
                completeObjectiveEvent.CompleteObjective
            );
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
