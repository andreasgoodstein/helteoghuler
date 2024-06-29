using Godot;
using HelteOgHulerClient.Utilities;
using HelteOgHulerShared.Models;
using System.Threading.Tasks;

namespace HelteOgHulerClient.Services;

public class AdventureService
{
    const string START_ADVENTURE_URL = "http://localhost:7111/Adventure/Start";

    private RequestNode startAdventureNode;

    public Task StartAdventure(Node httpRequestParent)
    {
        var taskSource = new TaskCompletionSource<bool>();

        startAdventureNode?.Clean();
        startAdventureNode = new RequestNode(httpRequestParent, ResponseType.JSONCallback);

        startAdventureNode.Response.JSONCallbackDelegate = (int result, int response_code, string[] headers, byte[] body) =>
        {
            // Expected Errors
            if (response_code == 400)
            {
                ResponseHandler.HandleGameStateResponse<HHError>(body, startAdventureNode);
                taskSource.SetResult(true);
            }
            // Unexpected Errors
            else if (response_code < 200 || response_code > 299)
            {
                GD.PrintErr("Network: Could not start Adventure");
                taskSource.SetResult(false);
            }
            // Success
            else
            {
                ResponseHandler.HandleGameStateResponse<Adventure>(body, startAdventureNode);
                taskSource.SetResult(true);
            }

            startAdventureNode?.Clean();
        };

        startAdventureNode.Request.Request(START_ADVENTURE_URL, startAdventureNode.Headers);

        return taskSource.Task;
    }
}
