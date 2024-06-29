using System;
using System.Threading.Tasks;
using Godot;
using HelteOgHulerClient.Utilities;

namespace HelteOgHulerClient.Services;

public static class LoginService
{
    const string LOGIN_URL = "http://localhost:7111/Login";
    const string SaveUserGuid = "globalThis?.localStorage?.setItem('userGuid','USER_GUID')";
    private static RequestNode loginNode;

    public static Task<string> Login(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<string>();

        loginNode?.Clean();
        loginNode = new RequestNode(httpRequestParent, ResponseType.StringCallback);

        loginNode.Response.TextCallbackDelegate = (int result, int response_code, string[] headers, string body) =>
        {
            if (response_code == 401)
            {
                // TODO: Handle wrong username
                GD.PrintErr("Login: Wrong Username");
                taskSource.SetResult("");
            }
            else if (response_code < 200 || response_code > 299)
            {
                GD.PrintErr("Network: Could not Login");
                taskSource.SetResult("");
            }
            else
            {
                GD.Print(body);
                JavaScript.Eval(SaveUserGuid.Replace("USER_GUID", body));

                taskSource.SetResult(body);
            }

            loginNode?.Clean();
        };

        loginNode.Request.Request(LOGIN_URL, loginNode.Headers);

        return taskSource.Task;
    }
}
