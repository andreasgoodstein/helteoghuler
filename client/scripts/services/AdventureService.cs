using Godot;
using HelteOgHulerShared.Models;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class AdventureService
{
    const string START_ADVENTURE_URL = "Adventure/Start";

    private RequestNode startAdventureNode;

    public Task StartAdventure(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<bool>();

        startAdventureNode?.Clean();
        startAdventureNode = new RequestNode(httpRequestParent, ResponseType.JSONCallback);

        startAdventureNode.SetErrorHandler(() =>
        {
            GD.PrintErr("Network: Could not start Adventure");
            taskSource.SetResult(false);

            startAdventureNode?.Clean();
            startAdventureNode = null;

        });

        startAdventureNode.SetResponseHandler((byte[] body) =>
        {
            ResponseHandler.HandleGameStateResponse<Adventure>(body);
            taskSource.SetResult(true);

            startAdventureNode?.Clean();
            startAdventureNode = null;
        });

        startAdventureNode.ExecuteRequest(START_ADVENTURE_URL);

        return taskSource.Task;
    }
}
