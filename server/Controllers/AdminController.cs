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
    private readonly string ERROR_401 = HHJsonSerializer.Serialize(new HHError { Message = "You're not The Marquee! Go away." });
    private readonly string ERROR_400 = HHJsonSerializer.Serialize(new HHError { Message = "LoginName missing" });

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
                Content = ERROR_401,
                StatusCode = 401,
            };
        }

        if (String.IsNullOrWhiteSpace(loginName))
        {
            return new ContentResult
            {
                Content = ERROR_400,
                StatusCode = 400,
            };
        }

        var newUser = _userLogic.AddUser(loginName);

        return HHJsonSerializer.Serialize(newUser);
    }
}
