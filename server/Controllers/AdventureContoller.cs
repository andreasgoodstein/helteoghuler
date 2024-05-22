using Microsoft.AspNetCore.Mvc;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Services;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdventureController : ControllerBase
{
    private readonly EventService _eventService;

    public AdventureController(EventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet(Name = "Count")]
    public Task<long> Count()
    {
        return _eventService.GetCount();
    }

    [HttpGet(Name = "Start")]
    public async Task<string> Start()
    {
        await _eventService.CreateAsync(new AdventureEvent() { Gold = 1 });
        return "OK";
    }
}