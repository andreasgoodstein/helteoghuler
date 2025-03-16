using HelteOgHulerServer.Events;
using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdventureController(
    AdventureLogic adventureLogic,
    GameStateLogic gameStateLogic,
    EventService eventService
) : ControllerBase
{
    [HttpGet(Name = "Start")]
    public async Task<ActionResult<string>> Start()
    {
        User user = (User)HttpContext.Items["User"]!;

        try
        {
            Adventure adventure = adventureLogic.GenerateAdventure(user.PlayerId);

            AdventureEvent_V1 adventureEvent = new()
            {
                Adventure = adventure,
                CreatedAt = DateTime.UtcNow,
                PlayerId = user.PlayerId,
            };

            await eventService.CreateAsync(adventureEvent);

            gameStateLogic.UpdateGameState(adventureEvent);

            return HHJsonSerializer.Serialize(adventureEvent.Adventure);
        }
        catch (InvalidOperationException exception)
        {
            return new ContentResult
            {
                Content = HHJsonSerializer.Serialize(new HHError { Message = exception.Message }),
                StatusCode = 400,
            };
        }
    }
}
