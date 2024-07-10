using Godot;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class GameStateService
{
    const string REFRESH_GAMESTATE_URL = "GameState";

    private RequestNode refreshGameStateNode;

    public Task RefreshGameState(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<bool>();

        refreshGameStateNode?.Clean();
        refreshGameStateNode = new RequestNode(httpRequestParent, ResponseType.JSONCallback);

        refreshGameStateNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not get GameState");
            taskSource.SetResult(false);

            refreshGameStateNode?.Clean();
            refreshGameStateNode = null;
        });

        refreshGameStateNode.SetResponseHandler((byte[] body) =>
        {
            ResponseHandler.HandleGameStateResponse(body);
            taskSource.SetResult(true);

            refreshGameStateNode?.Clean();
            refreshGameStateNode = null;
        });

        refreshGameStateNode.ExecuteRequest(REFRESH_GAMESTATE_URL);

        return taskSource.Task;
    }
}
