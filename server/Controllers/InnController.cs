using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class InnController : ControllerBase
{
    private readonly EventService _eventService;
    private readonly GameStateLogic _gameStateLogic;
    private readonly InnLogic _innLogic;

    public InnController(EventService eventService, GameStateLogic gameStateLogic, InnLogic innLogic)
    {
        _eventService = eventService;
        _gameStateLogic = gameStateLogic;
        _innLogic = innLogic;
    }

    [HttpGet(Name = "RecruitHero")]
    public async Task<ActionResult<string>> RecruitHero(Guid heroId)
    {
        User user = (User)HttpContext.Items["User"]!;

        try
        {
            Recruitment recruitment = _innLogic.RecruitHero(user.PlayerId, heroId);

            var recruitmentEvent = new RecruitHeroEvent_V1
            {
                CreatedAt = DateTime.UtcNow,
                Recruitment = recruitment
            };

            await _eventService.CreateAsync(recruitmentEvent);

            _gameStateLogic.UpdateGameState(recruitmentEvent);

            return HHJsonSerializer.Serialize(recruitment);
        }
        catch (InvalidDataException exception)
        {
            return new ContentResult
            {
                Content = HHJsonSerializer.Serialize(new HHError { Message = exception.Message }),
                StatusCode = 500
            };
        }
    }
}
