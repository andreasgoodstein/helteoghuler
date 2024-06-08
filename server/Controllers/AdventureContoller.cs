using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdventureController : ControllerBase
{
    private readonly AdventureLogic _adventureLogic;
    private readonly GameStateLogic _gameStateLogic;
    private readonly EventService _eventService;

    public AdventureController(AdventureLogic adventureLogic, GameStateLogic gameStateLogic, EventService eventService)
    {
        _adventureLogic = adventureLogic;
        _gameStateLogic = gameStateLogic;
        _eventService = eventService;
    }

    [HttpGet(Name = "Start")]
    public async Task<ActionResult<string>> Start()
    {
        if (!_adventureLogic.CanPlayerAdventureForth())
        {
            var error = new HHError
            {
                Message = "Your party cannot venture forth yet"
            };

            return new ContentResult
            {
                Content = HHJsonSerializer.Serialize(error),
                StatusCode = 400,
            };
        }

        Adventure adventure = _adventureLogic.GenerateAdventure();

        AdventureEvent adventureEvent = new AdventureEvent()
        {
            Adventure = adventure,
            CreatedAt = DateTime.UtcNow,
            PlayerId = _gameStateLogic.Get().Player.Id
        };

        await _eventService.CreateAsync(adventureEvent);

        _gameStateLogic.UpdateGameState(adventureEvent);

        return HHJsonSerializer.Serialize(adventure);
    }
}
