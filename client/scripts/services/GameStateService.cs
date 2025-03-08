using Godot;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class GameStateService : BaseService
{
    const string REFRESH_GAMESTATE_URL = "GameState";

    public Task RefreshGameState(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<bool>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not get GameState");
            taskSource.SetResult(false);
        });

        requestNode.SetResponseHandler((byte[] body) =>
        {
            ResponseHandler.HandleGameStateResponse(body);
            taskSource.SetResult(true);
        });

        requestNode.ExecuteRequest(REFRESH_GAMESTATE_URL);

        return taskSource.Task;
    }
}
