using Microsoft.AspNetCore.Mvc;
using HelteOgHulerShared;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdventureController : ControllerBase
{
    [HttpGet(Name = "Start")]
    public string Start()
    {
        return "OK";
    }
}