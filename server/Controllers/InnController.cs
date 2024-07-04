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
    private readonly string ERROR_400 = HHJsonSerializer.Serialize(new HHError { Message = "This deed has already been claimed" });

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

        // Move command check to logic layer (PlayerLogic.cs)
        if (_gameStateLogic.Get().PrivatePlayerDict.ContainsKey(user.PlayerId))
        {
            return new ContentResult
            {
                Content = ERROR_400,
                StatusCode = 400,
            };
        }

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
}
