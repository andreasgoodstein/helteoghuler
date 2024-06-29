using Godot;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class GameStateService
{
    const string REFRESH_GAMESTATE_URL = "http://localhost:7111/GameState";

    private RequestNode refreshGameStateNode;

    public Task RefreshGameState(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<bool>();

        refreshGameStateNode?.Clean();
        refreshGameStateNode = new RequestNode(httpRequestParent, ResponseType.JSONCallback);

        refreshGameStateNode.Response.JSONCallbackDelegate = (int result, int response_code, string[] headers, byte[] body) =>
        {
            if (response_code < 200 || response_code > 299)
            {
                GD.PrintErr("Network: Could not get GameState");
                taskSource.SetResult(false);
            }
            else
            {
                ResponseHandler.HandleGameStateResponse(body, refreshGameStateNode);
                taskSource.SetResult(true);
            }

            refreshGameStateNode?.Clean();
        };

        refreshGameStateNode.Request.Request(REFRESH_GAMESTATE_URL, refreshGameStateNode.Headers);

        return taskSource.Task;
    }
}
