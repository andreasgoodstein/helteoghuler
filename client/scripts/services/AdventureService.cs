using Godot;
using HelteOgHulerShared.Models;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class AdventureService : BaseService
{
    const string START_ADVENTURE_URL = "Adventure/Start";

    public Task<string> StartAdventure(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<string>();

        Clean(new RequestNode(httpRequestParent, ResponseType.JSONCallback));

        requestNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not start Adventure");
            taskSource.SetResult("Could not start Adventure.");

            Clean();
        });

        requestNode.SetResponseHandler((byte[] body) =>
        {
            var adventure = ResponseHandler.HandleGameStateResponse<Adventure>(body);
            taskSource.SetResult(adventure.Status);

            Clean();
        });

        requestNode.ExecuteRequest(START_ADVENTURE_URL);

        return taskSource.Task;
    }
}
