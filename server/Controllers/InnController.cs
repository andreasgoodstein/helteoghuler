using HelteOgHulerServer.Events;
using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class InnController(
    EventService eventService,
    GameStateLogic gameStateLogic,
    InnLogic innLogic
) : ControllerBase
{
    [HttpGet(Name = "RecruitHero")]
    public async Task<ActionResult<string>> RecruitHero(Guid heroId)
    {
        User user = (User)HttpContext.Items["User"]!;

        try
        {
            Recruitment recruitment = innLogic.RecruitHero(user.PlayerId, heroId);

            var recruitmentEvent = new RecruitHeroEvent_V1
            {
                CreatedAt = DateTime.UtcNow,
                Recruitment = recruitment,
            };

            await eventService.CreateAsync(recruitmentEvent);

            gameStateLogic.UpdateGameState(recruitmentEvent);

            return HHJsonSerializer.Serialize(recruitment);
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

    [HttpGet(Name = "UpgradeInn")]
    public async Task<ActionResult<string>> UpgradeInn(InnUpgradeName upgrade)
    {
        User user = (User)HttpContext.Items["User"]!;

        try
        {
            InnUpgrade innUpgrade = innLogic.UpgradeInn(user.PlayerId, upgrade);

            var upgradeEvent = new UpgradeInnEvent_V1
            {
                CreatedAt = DateTime.UtcNow,
                Upgrade = innUpgrade,
            };

            await eventService.CreateAsync(upgradeEvent);

            gameStateLogic.UpdateGameState(upgradeEvent);

            return HHJsonSerializer.Serialize(upgradeEvent.Upgrade);
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
