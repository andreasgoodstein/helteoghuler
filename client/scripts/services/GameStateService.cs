using Godot;
using HelteOgHulerClient.Utilities;
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

        refreshGameStateNode.SetResponseHandler((int result, int response_code, string[] headers, byte[] body) =>
        {
            // Unauthorized
            if (response_code == 401)
            {
                GD.PrintErr("Auth: Unauthorized");
                httpRequestParent.GetTree().ChangeScene("res://scenes/LoginMenuScene.tscn");
            }
            // Unexpected Errors
            else if (response_code < 200 || response_code > 299)
            {
                GD.PrintErr("Network: Could not get GameState");
                taskSource.SetResult(false);
            }
            // Success
            else
            {
                ResponseHandler.HandleGameStateResponse(body, refreshGameStateNode);
                taskSource.SetResult(true);
            }

            refreshGameStateNode?.Clean();
        });

        refreshGameStateNode.ExecuteRequest(REFRESH_GAMESTATE_URL);

        return taskSource.Task;
    }
}
