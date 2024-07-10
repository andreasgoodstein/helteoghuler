using Godot;
using HelteOgHulerShared.Models;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class AdventureService : BaseService
{
    const string START_ADVENTURE_URL = "Adventure/Start";

    public Task StartAdventure(Node httpRequestParent)
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
            ResponseHandler.HandleGameStateResponse<Adventure>(body);
            taskSource.SetResult(true);

            Clean();
        });

        requestNode.ExecuteRequest(START_ADVENTURE_URL);

        return taskSource.Task;
    }
}
