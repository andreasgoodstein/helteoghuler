using Godot;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class PlayerService : BaseService
{
    const string NEW_PLAYER_URL = "Player/New";

    public Task CreateNewPlayer(Node httpRequestParent, string innName, string playerName)
    {
        var taskSource = new TaskCompletionSource<bool>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not start Adventure");
            taskSource.SetResult(false);

            Clean();
        });

        requestNode.SetResponseHandler((byte[] body) =>
        {
            ResponseHandler.HandleGameStateResponse(body);
            taskSource.SetResult(true);

            Clean();
        });

        requestNode.ExecuteRequest(NEW_PLAYER_URL + $"?innName={innName}&playerName={playerName}");

        return taskSource.Task;
    }
}
