using HelteOgHulerServer.Logic;
using HelteOgHulerServer.Models;
using HelteOgHulerServer.Services;
using HelteOgHulerShared.Models;
using HelteOgHulerShared.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace HelteOgHulerServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdminController : ControllerBase
{
    private readonly UserLogic _userLogic;

    public AdminController(UserLogic userLogic)
    {
        _userLogic = userLogic;
    }

    [HttpGet(Name = "NewUser")]
    public ActionResult<string> NewUser(string loginName)
    {
        User user = (User)HttpContext.Items["User"]!;

        if (user?.IsAdmin != true)
        {
            return new ContentResult
            {
                Content = "You're not The Marquee! Go away.",
                StatusCode = 401,
            };
        }

        if (String.IsNullOrWhiteSpace(loginName))
        {
            return new ContentResult
            {
                Content = "LoginName missing",
                StatusCode = 400,
            };
        }

        var newUser = _userLogic.AddUser(loginName);

        return HHJsonSerializer.Serialize(newUser);
    }
}
