using System.Threading.Tasks;
using Godot;
using HelteOgHulerShared.Models;

namespace HelteOgHulerClient.Services;

public class PlayerService : BaseService
{
    const string NEW_PLAYER_URL = "Player/New";
    const string COMPLETE_OBJECTIVE_URL = "Player/CompleteObjective";

    public Task CreateNewPlayer(Node httpRequestParent, string innName, string playerName)
    {
        var taskSource = new TaskCompletionSource<bool>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not start Adventure");
            taskSource.SetResult(false);
        });

        requestNode.SetResponseHandler(body =>
        {
            ResponseHandler.HandleGameStateResponse(body);
            taskSource.SetResult(true);
        });

        requestNode.ExecuteRequest(NEW_PLAYER_URL + $"?innName={innName}&playerName={playerName}");

        return taskSource.Task;
    }

    public Task CompleteObjective(Node httpRequestParent, PlayerObjective objective)
    {
        var taskSource = new TaskCompletionSource<bool>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not complete objective");
            taskSource.SetResult(false);
        });

        requestNode.SetResponseHandler(body =>
        {
            ResponseHandler.HandleGameStateResponse<CompleteObjective>(body);
            taskSource.SetResult(true);
        });

        requestNode.ExecuteRequest(COMPLETE_OBJECTIVE_URL + $"?objective={objective}");

        return taskSource.Task;
    }
}
