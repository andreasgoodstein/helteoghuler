using Microsoft.AspNetCore.Mvc;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;

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
        if (!_adventureLogic.CanPlayerGenerateAdventure())
        {
            return new ContentResult() { StatusCode = 400, Content = "Your party cannot venture forth" };
        }

        Adventure adventure = _adventureLogic.GenerateAdventure();

        await _eventService.CreateAsync(new AdventureEvent()
        {
            Adventure = adventure,
            CreatedAt = DateTime.UtcNow,
            PlayerId = _gameStateLogic.GetGameState().Player.Id
        });

        GameState newGameState = _gameStateLogic.UpdateGameState(new AdventureEvent { Adventure = adventure });

        return HHJsonSerializer.Serialize(newGameState);
    }
}